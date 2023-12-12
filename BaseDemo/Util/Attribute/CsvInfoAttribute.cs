using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDemo.Util.Attribute
{
    public sealed class CsvInfoAttribute : System.Attribute
    {
        public int Index;
        public string TitleName;
        public string ColumnWidth;
        public string ColumnName;
    }
}
