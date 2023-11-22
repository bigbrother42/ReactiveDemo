using ReactiveDemo.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Managers
{
    public class LogFilenameProvider : ILogFilenameProvider
    {
        private const string LOGFILE_DEFAULT_FILEPATH = @"..\logs";
        private const string LOGFILE_DEFAULT_APPENDDATEFORMAT = "yyyyMMdd";
        private const string LOGFILE_DEFAULT_EXTENSION = ".log";
        private const string LOGFILE_DEFAULT_SEPARATOR = ".";

        /// <summary>
        /// ログファイル名を取得する
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="isAppendDate"></param>
        /// <param name="appendDateFormat"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public string GetLogFilename(string path = null
            , string name = null
            , bool isAppendDate = true
            , string appendDateFormat = null
            , string extension = null)
        {
            StringBuilder filePathStringBuilder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(path))
            {
                path = LOGFILE_DEFAULT_FILEPATH;
            }
            filePathStringBuilder.Append(path);
            filePathStringBuilder.Append(Path.DirectorySeparatorChar);

            StringBuilder fileNameStringBuilder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(name))
            {
                name = GetLoggerName();
            }
            fileNameStringBuilder.Append(name);
            fileNameStringBuilder.Append(LOGFILE_DEFAULT_SEPARATOR);

            if (isAppendDate)
            {
                if (string.IsNullOrWhiteSpace(appendDateFormat))
                {
                    appendDateFormat = LOGFILE_DEFAULT_APPENDDATEFORMAT;
                }
                fileNameStringBuilder.Append(DateTime.Now.ToString(appendDateFormat));
            }

            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = LOGFILE_DEFAULT_EXTENSION;
            }

            fileNameStringBuilder.Append(extension);
            filePathStringBuilder.Append(fileNameStringBuilder.ToString());

            return filePathStringBuilder.ToString();
        }

        /// <summary>
        /// ロギング処理の名前を取得する
        /// </summary>
        /// <returns></returns>
        private string GetLoggerName()
        {
            var assemblyInfo = new AssemblyInfoUtil(Assembly.GetEntryAssembly());
            return assemblyInfo.Title;
        }
    }
}
