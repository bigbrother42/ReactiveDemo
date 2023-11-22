

using log4net.Appender;
using log4net.Layout;
using static log4net.Appender.FileAppender;
using static log4net.Appender.RollingFileAppender;

namespace ReactiveDemo.Managers
{
    public class LogAppenderProvider : ILogAppenderProvider
    {
        private ILogLayoutProvider _logLayoutProvider = null;
        private ILogFilenameProvider _logFilenameProvider = null;

        public LogAppenderProvider(ILogLayoutProvider logLayoutProvider, ILogFilenameProvider logFilenameProvider)
        {
            _logLayoutProvider = logLayoutProvider;
            _logFilenameProvider = logFilenameProvider;
        }

        public IAppender CreateRollingFileAppender(
            ILayout layout = null,
            string fileName = null,
            bool appendToFile = true,
            int maxSizeRollBackups = 20,
            string maxFileSize = "50MB",
            RollingMode rollingStyle = RollingMode.Size,
            bool staticLogFilename = true,
            bool preserveLogFileNameExtension = true)
        {
            if (layout == null)
            {
                layout = _logLayoutProvider.CreatePatternLayout();
            }

            var appender = new RollingFileAppender
            {
                AppendToFile = appendToFile,
                File = _logFilenameProvider.GetLogFilename(name: fileName),
                Layout = layout,
                MaxSizeRollBackups = maxSizeRollBackups,
                MaximumFileSize = maxFileSize,
                RollingStyle = rollingStyle,
                StaticLogFileName = staticLogFilename,
                PreserveLogFileNameExtension = preserveLogFileNameExtension,
                LockingModel = new MinimalLock()
            };

            appender.ActivateOptions();

            return appender;
        }

        public IAppender CreateMemoryAppender(ILayout layout = null)
        {
            if (layout == null)
            {
                layout = _logLayoutProvider.CreatePatternLayout();
            }

            MemoryAppender memory = new MemoryAppender()
            {
                Layout = layout
            };
            memory.ActivateOptions();
            return memory;
        }

        public IAppender CreateConsoleAppender(ILayout layout = null)
        {
            if (layout == null)
            {
                layout = _logLayoutProvider.CreatePatternLayout();
            }

            ConsoleAppender console = new ConsoleAppender
            {
                Layout = layout
            };
            console.ActivateOptions();

            return console;
        }
    }
}
