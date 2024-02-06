using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDemo.ClientBase
{
    public abstract class SystemConfigAbstract : ISystemConfig
    {
        protected string FunctionNo = string.Empty;

        protected string ItemNo = string.Empty;

        protected string ItemName = string.Empty;

        public virtual void Save()
        {
            
        }
    }
}
