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
    }
}
