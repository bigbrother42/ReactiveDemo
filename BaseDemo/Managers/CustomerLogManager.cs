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

namespace BaseDemo.Managers
{
    public class CustomerLogManager
    {
        private const int DEFAULT_MAX_FILE_AGE_IN_DAYS = 30;

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
        public Hierarchy CreateLogger(string xmlConfigFilePath)
        {
            var logRepository = (Hierarchy)LogManager.GetRepository();
            var configFileInfo = new FileInfo(xmlConfigFilePath);
            XmlConfigurator.ConfigureAndWatch(configFileInfo);
            return logRepository;
        }
    }
}
