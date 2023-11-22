using log4net.Appender;
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static log4net.Appender.RollingFileAppender;

namespace ReactiveDemo.Managers
{
    public interface ILogAppenderProvider
    {
        IAppender CreateRollingFileAppender(ILayout layout = null, string fileName = null, bool appendToFile = true, int maxSizeRollBackups = 20, string maxFileSize = "50MB", RollingMode rollingStyle = RollingMode.Size, bool staticLogFilename = true, bool preserveLogFileNameExtension = true);

        IAppender CreateMemoryAppender(ILayout layout = null);

        IAppender CreateConsoleAppender(ILayout layout = null);
    }
}
