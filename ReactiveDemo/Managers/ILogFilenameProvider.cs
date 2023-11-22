using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Managers
{
    public interface ILogFilenameProvider
    {
        string GetLogFilename(string path = null, string name = null, bool isAppendDate = true, string appendDateFormat = null, string extension = null);
    }
}
