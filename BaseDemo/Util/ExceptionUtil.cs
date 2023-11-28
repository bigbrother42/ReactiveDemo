using BaseDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BaseDemo.Data.Constants.Constants;

namespace BaseDemo.Util
{
    public class ExceptionUtil
    {
        public static AppErrorInfo ConvertToAppError(Exception ex, AppStatus appStatus = AppStatus.CLIENT_SYSTEM_ERROR)
        {
            return new AppErrorInfo
            {
                ErrorCode = ((int)appStatus).ToString(),
                ErrorMessage = ex.Message,
                ExceptionClass = ex.GetType()?.FullName,
                ExceptionCause = ex.InnerException?.Message,
                ExceptionStackTrace = ex.StackTrace
            };
        }
    }
}
