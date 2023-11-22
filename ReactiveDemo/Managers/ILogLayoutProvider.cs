using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Managers
{
    public interface ILogLayoutProvider
    {
        ILayout CreatePatternLayout(string pattern = null);
    }
}
