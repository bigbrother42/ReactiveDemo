using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDemo.GlobalData
{
    public static class GlobalData
    {
        public static SqliteConnection DbConnection { get; set; }

        public static string SystemTheme { get; set; } = "Light.Olive";

        public static readonly string CSV_PATH = $@"{System.Windows.Forms.Application.StartupPath}\..\data\csv\note";

        public static readonly string IMAGE_PATH = $@"{System.Windows.Forms.Application.StartupPath}\..\data\image\note";
    }
}
