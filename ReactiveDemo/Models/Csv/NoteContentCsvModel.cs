using BaseDemo.Util.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Models.Csv
{
    public class NoteContentCsvModel
    {
        [CsvInfo(Index = 1, TitleName = "user_id", ColumnName = "", ColumnWidth = "80")]
        public int UserId { get; set; }

        [CsvInfo(Index = 2, TitleName = "type_id", ColumnName = "", ColumnWidth = "80")]
        public int TypeId { get; set; }

        [CsvInfo(Index = 3, TitleName = "content_id", ColumnName = "", ColumnWidth = "80")]
        public int ContentId { get; set; }

        [CsvInfo(Index = 4, TitleName = "category_id", ColumnName = "", ColumnWidth = "80")]
        public int CategoryId { get; set; }

        [CsvInfo(Index = 5, TitleName = "content", ColumnName = "", ColumnWidth = "80")]
        public string Content { get; set; }
    }
}
