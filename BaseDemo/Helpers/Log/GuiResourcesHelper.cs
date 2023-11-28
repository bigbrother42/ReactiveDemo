using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BaseDemo.Helpers.Log
{
    public class GuiResourcesHelper
    {
        [DllImport("user32.dll")]
        private static extern uint GetGuiResources(IntPtr hProcess, uint uiFlags);

        /// <summary>
        ///
        /// </summary>
        public const uint GR_GDIOBJECTS = 0;

        /// <summary>
        ///
        /// </summary>
        public const uint GR_USEROBJECTS = 1;

        private uint _uiFlags = GR_GDIOBJECTS;

        /// <summary>
        ///
        /// </summary>
        /// <param name="uiFlags"></param>
        public GuiResourcesHelper(uint uiFlags)
        {
            _uiFlags = uiFlags;
        }

        /// <summary>
        ///
        /// </summary>
        public override string ToString()
        {
            using (Process currentProcess = Process.GetCurrentProcess())
            {
                var count = GetGuiResources(currentProcess.Handle, _uiFlags);
                return count.ToString();
            }
        }
    }
}
