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
        [CsvInfo(Index = 1, TitleName = "user_id", ColumnName = "", ColumnWidth = "80")]
        public int UserId { get; set; }

        [CsvInfo(Index = 2, TitleName = "type_id", ColumnName = "", ColumnWidth = "80")]
        public int TypeId { get; set; }

        [CsvInfo(Index = 3, TitleName = "type_name", ColumnName = "", ColumnWidth = "80")]
        public string TypeName { get; set; }

        [CsvInfo(Index = 4, TitleName = "display_order", ColumnName = "", ColumnWidth = "80")]
        public string DisplayOrder { get; set; }

        [CsvInfo(Index = 5, TitleName = "description", ColumnName = "", ColumnWidth = "80")]
        public string Description { get; set; }
    }
}
