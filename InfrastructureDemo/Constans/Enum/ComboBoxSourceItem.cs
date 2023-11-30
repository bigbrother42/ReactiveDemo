using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo.Constants.Enum
{
    public class ComboBoxSourceItem
    {
        public int Value { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Code { get; set; }
        public string DescriptionEn { get; set; }
        public bool IsKarte { get; set; }
        public bool IsIji { get; set; }
    }

    public class ListViewSourceItem : ComboBoxSourceItem
    {
        public bool IsSelected { get; set; }
    }
}
