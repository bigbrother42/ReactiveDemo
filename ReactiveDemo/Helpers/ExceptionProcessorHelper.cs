using ReactiveDemo.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Helpers
{
    public class ExceptionProcessorHelper
    {
        public static void Handle(Exception exception)
        {
            var message = string.Empty;
            try
            {
                if (exception == null)
                {
                    return;
                }
                else
                {
                    message =
                        $"Exception: {exception.GetType().Name}, {Environment.NewLine}" +
                        $"Message: {exception.Message}, {Environment.NewLine}" +
                        $"StackTrace: {Environment.NewLine}" +
                        $"{exception.StackTrace}{Environment.NewLine}";

                    LogUtil.Instance?.Error(exception?.Message);
                    LogUtil.Instance?.Debug(exception?.ToString());
                    LogUtil.Instance?.Debug(message);

                    Trace.TraceError(exception.Message);
                    Trace.TraceError(exception.ToString());
                    Trace.TraceError(message);
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    LogUtil.Instance?.Error(ex?.Message);
                    LogUtil.Instance?.Debug(ex?.ToString());
                    LogUtil.Instance?.Debug(message);
                }
            }
        }
    }
}
