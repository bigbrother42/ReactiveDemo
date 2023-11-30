using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static InfrastructureDemo.Constants.Constants;

namespace ReactiveDemo.Util.Login
{
    public class LoginUtil
    {
        public static bool ValidateAsciiOnly(string str, string caption, out string message)
        {
            var validationResult = StringIsAsciiOnly(str);
            if (!validationResult)
            {
                message = string.Format(LoginConstants.VALIDATION_ERROR_ASCIIONLY, caption);
            }
            else
            {
                message = null;
            }

            return validationResult;
        }

        public static bool StringIsAsciiOnly(string str)
        {
            return Regex.IsMatch(str, @"^[\x00-\x7F]+$");
        }
    }
}
