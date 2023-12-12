using BaseDemo.Util.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Models.Csv
{
    public class NoteTypeCsvModel
    {
        [CsvInfo(Index = 1, TitleName = "UserId", ColumnName = "", ColumnWidth = "80")]
        public int UserId { get; set; }

        [CsvInfo(Index = 1, TitleName = "TypeId", ColumnName = "", ColumnWidth = "80")]
        public int TypeId { get; set; }

        [CsvInfo(Index = 1, TitleName = "TypeName", ColumnName = "", ColumnWidth = "80")]
        public string TypeName { get; set; }

        [CsvInfo(Index = 1, TitleName = "Description", ColumnName = "", ColumnWidth = "80")]
        public string Description { get; set; }
    }
}
