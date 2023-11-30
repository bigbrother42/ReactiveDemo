using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InfrastructureDemo.Constants.Enum.EnumExtension;

namespace InfrastructureDemo.Constans.Enum
{
    public class EnumConstants
    {
        public enum MessageBoxType
        {
            [Description("Normal"), Code("Normal")]
            Normal = 0,

            [Description("OK"), Code("OK")]
            OK = 1,
        }
    }
}
