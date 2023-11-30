using DataDemo.WebDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDemo.GlobalData
{
    public class LoginInfo
    {
        public static bool IsAdmin { get; set; }

        public static UserBasicInfoWebDto UserBasicInfo { get; set; }
    }
}
