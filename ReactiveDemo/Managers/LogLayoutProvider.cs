using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Managers
{
    public class LogLayoutProvider : ILogLayoutProvider
    {
        private const string DEFAULT_CONVERSION_PATTERN = "%d{yyyy/MM/dd HH:mm:ss.fffffff},%property{log4net:HostName},,%-5p,%C,%M,%m%n";

        private string _conversionPattern;

        /// <summary>
        ///
        /// </summary>
        /// <param name="pattern"></param>
        public LogLayoutProvider(string pattern = DEFAULT_CONVERSION_PATTERN)
        {
            _conversionPattern = pattern;
        }

        /// <summary>
        /// ログメッセージ出力パターンを作成する
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public ILayout CreatePatternLayout(string pattern = null)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                pattern = _conversionPattern;
            }
            PatternLayout patternLayout = new PatternLayout(pattern);
            patternLayout.ActivateOptions();
            return patternLayout;
        }
    }
}
