using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveDemo.Managers
{
    public class CustomerLogManager
    {
        private const int DEFAULT_MAX_FILE_AGE_IN_DAYS = 30;

        private readonly SemaphoreSlim _repositoryConfigurationChangedSemaphore = new SemaphoreSlim(1);
        private int _maxFileAgeInDays = DEFAULT_MAX_FILE_AGE_IN_DAYS;

        #region Singleton

        static CustomerLogManager()
        {
        }

        private CustomerLogManager()
        {
            //var repository = LogManager.GetRepository();
            //repository.ConfigurationChanged += Repository_ConfigurationChanged;
            // TODO
            //_maxFileAgeInDays = EmAppSettingsUtil.GetInt(EmBaseConfigKeys.LogManagerBackupMaxDays, DEFAULT_MAX_FILE_AGE_IN_DAYS);
        }

        /// <summary>
        ///
        /// </summary>
        public static CustomerLogManager Instance { get; } = new CustomerLogManager();

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="loggerName"></param>
        /// <param name="levelName"></param>
        /// <param name="appenders"></param>
        /// <returns></returns>
        public Hierarchy CreateLogger(string loggerName, string levelName, params IAppender[] appenders)
        {
            var logRepository = (Hierarchy)LogManager.CreateRepository(loggerName);

            BasicConfigurator.Configure(logRepository);
            logRepository.Root.RemoveAllAppenders();
            logRepository.Root.Level = logRepository.Root.Hierarchy.LevelMap[levelName];

            if (appenders != null)
            {
                foreach (var appender in appenders)
                {
                    logRepository.Root.AddAppender(appender);
                }
            }

            return logRepository;
        }

        /// <summary>
        ///
        /// </summary>
        public Hierarchy CreateLogger(string xmlConfigFilePath)
        {
            var logRepository = (Hierarchy)LogManager.GetRepository();
            var configFileInfo = new FileInfo(xmlConfigFilePath);
            XmlConfigurator.ConfigureAndWatch(configFileInfo);
            return logRepository;
        }

        /// <summary>
        ///
        /// </summary>
        public Hierarchy CreateLogger(Stream stream)
        {
            var logRepository = (Hierarchy)LogManager.GetRepository();
            XmlConfigurator.Configure(logRepository, stream);
            return logRepository;
        }

        /// <summary>
        ///
        /// </summary>
        public Hierarchy CreateLogger(string loggerName, string xmlConfigFilePath)
        {
            var logRepository = (Hierarchy)LogManager.CreateRepository(loggerName);
            var configFileInfo = new FileInfo(xmlConfigFilePath);
            XmlConfigurator.ConfigureAndWatch(logRepository, configFileInfo);
            return logRepository;
        }

        /// <summary>
        ///
        /// </summary>
        public Hierarchy CreateLogger(string loggerName, Stream stream)
        {
            var logRepository = (Hierarchy)LogManager.CreateRepository(loggerName);
            XmlConfigurator.Configure(logRepository, stream);
            return logRepository;
        }

        /// <summary>
        ///
        /// </summary>
        public ILog GenerateLogger(string loggerName, Level logLevel, string conversionPattern = null)
        {
            var logLayoutProvider = new LogLayoutProvider(conversionPattern);
            var logFilenameProvider = new LogFilenameProvider();
            var appenderProvider = new LogAppenderProvider(logLayoutProvider, logFilenameProvider);

            IAppender IAppender1 = appenderProvider.CreateRollingFileAppender(fileName: loggerName);
            IAppender IAppender2 = appenderProvider.CreateMemoryAppender();
            IAppender IAppender3 = appenderProvider.CreateConsoleAppender();

            var hierarchy = CreateLogger(loggerName, logLevel.ToString(), IAppender1, IAppender2, IAppender3);
            return LogManager.GetLogger(hierarchy.Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        ///
        /// </summary>
        public async Task CompressOldLogFilesAsync()
        {
            var logDirectoryList = new List<string>();
            var repo = LogManager.GetRepository();
            var loggers = repo.GetAppenders();
            foreach (var logger in loggers)
            {
                if (logger is FileAppender)
                {
                    var logFile = ((FileAppender)logger).File;
                    if (!string.IsNullOrWhiteSpace(logFile))
                    {
                        string logDirectory = Path.GetDirectoryName(logFile);
                        logDirectoryList.Add(logDirectory);
                    }
                }
            }

            if (logDirectoryList != null && logDirectoryList.Count > 0)
            {
                await CompressOldLogFilesAsync(logDirectoryList).ConfigureAwait(false);
            }
        }

        private async void Repository_ConfigurationChanged(object sender, System.EventArgs e)
        {
            if (_repositoryConfigurationChangedSemaphore.CurrentCount == 0) return;
            await _repositoryConfigurationChangedSemaphore.WaitAsync();
            await CompressOldLogFilesAsync().ConfigureAwait(false);
        }

        private async Task CompressOldLogFilesAsync(List<string> logDirectoryList)
        {
            if (logDirectoryList != null && logDirectoryList.Count > 0)
            {
                foreach (var logDirectory in logDirectoryList)
                {
                    await CompressOldLogFilesAsync(logDirectory);
                }
            }
        }

        private async Task CompressOldLogFilesAsync(string logDirectory)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(logDirectory))
                {
                    if (Directory.Exists(logDirectory))
                    {
                        var files = Directory.EnumerateFiles(logDirectory, "*.*", SearchOption.AllDirectories);
                        if (files != null)
                        {
                            foreach (var file in files)
                            {
                                var fileInfo = new FileInfo(file);
                                if (!fileInfo.Exists || file.Length <= 0)
                                {
                                    continue;
                                }
                                var lastWriteDate = fileInfo.LastWriteTime.Date;
                                var currentDate = DateTime.Now.Date;
                                var span = currentDate - lastWriteDate;
                                if (span.TotalDays == 0)
                                {
                                    continue;
                                }
                                else if (span.TotalDays > _maxFileAgeInDays)
                                {
                                    File.Delete(file);
                                    continue;
                                }
                                else
                                {
                                    string extension = Path.GetExtension(file);
                                    switch (extension)
                                    {
                                        case ".zip":
                                        case ".gz":
                                            break;

                                        default:
                                            string directory = Path.GetDirectoryName(file);
                                            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                                            string zipFilePath = Path.Combine(directory, fileNameWithoutExt + ".zip");
                                            await CompressFileAsync(file, zipFilePath);
                                            var zipFileInfo = new FileInfo(zipFilePath);
                                            if (zipFileInfo.Exists && zipFileInfo.Length > 0)
                                            {
                                                File.SetCreationTime(zipFilePath, fileInfo.CreationTime);
                                                File.SetLastAccessTime(zipFilePath, fileInfo.LastAccessTime);
                                                File.SetLastWriteTime(zipFilePath, fileInfo.LastWriteTime);
                                                File.Delete(file);
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }

        private async Task CompressFileAsync(string file, string zipFilePath)
        {
            try
            {
                await Task.Run(() =>
                {
                    using (var zip = new Ionic.Zip.ZipFile(zipFilePath))
                    {
                        zip.AddFile(file, string.Empty);
                        zip.Save();
                    }
                });
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }
    }
}
