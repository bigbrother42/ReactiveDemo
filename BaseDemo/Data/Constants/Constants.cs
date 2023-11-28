using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BaseDemo.Data.Constants.EnumExtension;

namespace BaseDemo.Data.Constants
{
    public class Constants
    {
        public enum AppStatus : int 
        {
            /// <summary>
            ///   Normal
            /// </summary>
            [Description("Normal"), Code("0")]
            OK_KIBAN = 0,

            /// <summary>
            ///  password check missing
            /// </summary>
            [Description("password check missing"), Code("11")]
            PASSWORD_CHECK_MISSING = 11,

            /// <summary>
            ///   password check invalid
            /// </summary>
            [Description("password check invalid"), Code("12")]
            PASSWORD_CHECK_INVALID = 12,

            /// <summary>
            ///   device check missing
            /// </summary>
            [Description("device check missing"), Code("21")]
            DEVICE_CHECK_MISSING = 21,

            /// <summary>
            ///   client system error
            /// </summary>
            [Description("client system error"), Code("9071")]
            CLIENT_SYSTEM_ERROR = 9071,

            /// <summary>
            ///   database error
            /// </summary>
            [Description("database error"), Code("9100")]
            DATABASE_ERROR = 9100,
        }
    }
}
