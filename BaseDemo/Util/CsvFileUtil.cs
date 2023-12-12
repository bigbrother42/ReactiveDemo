using BaseDemo.Util.Attribute;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDemo.Util
{
    public class CsvFileUtil<T>
    {
        public static readonly string HALF_BLANK = " ";
        public static readonly string EngComma = ",";

        private static Dictionary<int, CsvInfoAttribute> BuildRelationship(Type type)
        {
            Dictionary<int, CsvInfoAttribute> map = new Dictionary<int, CsvInfoAttribute>();
            foreach (var item in type.GetProperties())
            {
                object[] obj = item.GetCustomAttributes(true);
                foreach (var item1 in obj)
                {
                    if (item1 is CsvInfoAttribute cia)
                    {
                        cia.ColumnName = item.Name;
                        map.Add(cia.Index, cia);
                        break;
                    }
                }
            }

            return map.OrderBy(x => x.Key).ToDictionary(k => k.Key, v => v.Value);
        }

        private static string GetCsvHeader(Dictionary<int, CsvInfoAttribute> map)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in map)
            {
                sb.Append(string.Concat(item.Value.TitleName, EngComma));
            }

            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        private static string GetCsvBodyLine(Dictionary<int, CsvInfoAttribute> map, T t)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kv in map)
            {
                var item = t.GetType().GetProperties().FirstOrDefault(x => x.Name == kv.Value.ColumnName);
                sb.Append(string.Concat(PlusDoubleQuotationMark(item.GetValue(t).ToString()), EngComma));
            }

            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        private static string GetCsvColWidth(Dictionary<int, CsvInfoAttribute> map)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in map)
            {
                sb.Append(string.Concat(item.Value.ColumnWidth, EngComma));
            }

            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        private static string GetCsvFilePath(string directory, string fileName)
        {
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (fileName.LastIndexOf('\\') != -1)
            {
                fileName = fileName.Substring(fileName.LastIndexOf('\\'));
            }

            return $"{directory}\\{fileName}_{DateTime.Now:yyyyMMddHHmmss}.csv";
        }

        public static string CsvFileGenerate(List<T> genericList, string directory, string fileName,
            bool isTitle = false, string TiContent = null,
            bool isOpenCsvView = true, string width = "1000", string hight = "900", bool isHorizontal = true, bool isAdjust = true)
        {
            string csvFile = GetCsvFilePath(directory, fileName);

            using (FileStream file = new FileStream(csvFile, FileMode.CreateNew))
            {
                using (StreamWriter writer = new StreamWriter(file, Encoding.GetEncoding("Shift-JIS")))
                {
                    if (isTitle)
                    {
                        writer.WriteLine(TiContent);
                    }

                    var map = BuildRelationship(typeof(T));
                    writer.WriteLine(GetCsvHeader(map));
                    foreach (var item in genericList)
                    {
                        writer.WriteLine(GetCsvBodyLine(map, item));
                    }

                    if (!isOpenCsvView) return csvFile;

                    OpenCsvFile(csvFile, GetCsvColWidth(map), width, hight, isHorizontal, isAdjust);
                }
            }

            return csvFile;
        }

        private static string PlusDoubleQuotationMark(string originStr)
        {
            return "\"" + originStr + "\"";
        }

        private static void OpenCsvFile(string csvFile, string columnWidth = "", string width = "1000", string hight = "900", bool isHorizontal = true, bool isAdjust = true)
        {
            var csvViewer = AppDomain.CurrentDomain.BaseDirectory + "CsvViewer.exe";
            var arguments = PlusDoubleQuotationMark(csvFile);

            if (!string.IsNullOrEmpty(columnWidth))
            {
                arguments = string.Concat(arguments, HALF_BLANK, "-ColumnWidth=", columnWidth);
            }

            if (isHorizontal)
            {
                arguments = string.Concat(arguments, HALF_BLANK, "-PrintDirection=Horizontal");
            }

            if (isAdjust)
            {
                arguments = string.Concat(arguments, HALF_BLANK, "-PrintAdjustment=Adjust");
            }

            if (!string.IsNullOrEmpty(width))
            {
                arguments = string.Concat(arguments, HALF_BLANK, "-Width=", width);
            }

            if (!string.IsNullOrEmpty(hight))
            {
                arguments = string.Concat(arguments, HALF_BLANK, "-Hight=", hight);
            }

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = csvViewer,
                Arguments = arguments
            };

            Process.Start(info);
        }
    }
}
