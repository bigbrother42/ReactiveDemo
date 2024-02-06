using BaseDemo.ClientBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDemo.SystemConfig
{
    public class SC_SystemTheme : SystemConfigAbstract
    {
        public SC_SystemTheme()
        {
            FunctionNo = "LN010001";
            ItemNo = "SystemColor";
            ItemName = "theme name of system";
        }

        public override void Save()
        {
            base.Save();


        }
    }
}
