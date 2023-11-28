using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDemo.Helpers.Log
{
    public class CurrentProcessHelper
    {
        private string _propertyName = string.Empty;

        /// <summary>
        ///
        /// </summary>
        /// <param name="propertyName"></param>
        public CurrentProcessHelper(string propertyName)
        {
            _propertyName = propertyName;
        }

        /// <summary>
        ///
        /// </summary>
        public override string ToString()
        {
            using (Process currentProcess = Process.GetCurrentProcess())
            {
                object value = typeof(Process).GetProperty(_propertyName).GetValue(currentProcess, null);
                return value.ToString();
            }
        }
    }
}
