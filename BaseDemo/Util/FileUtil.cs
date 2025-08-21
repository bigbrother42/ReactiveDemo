using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BaseDemo.Util
{
    public class FileUtil
    {
        public static readonly string FILE_EXTENSION = ".csv";
        public static readonly string ENGLISH_COMMA = ",";
        public static readonly string HALF_BLANK = " ";

        private static readonly HashSet<string> ImageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp",
            ".png", ".gif", ".bmp", ".tiff", ".tif",
            ".webp", ".ico", ".svg", ".heic", ".heif",
            ".raw", ".cr2", ".nef", ".arw", ".dng",
        };

        public static bool IsImageFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return false;

            if (!File.Exists(filePath)) return false;

            var extension = Path.GetExtension(filePath);
            return ImageExtensions.Contains(extension);
        }

        public static BitmapSource FilePath2BitmapSource(string filePath, int width)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return null;

            try
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
                bitmapImage.DecodePixelWidth = width;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DataTable ConvertCsvToDataTable(string filePath)
        {
            DataTable dt = new DataTable();
            string strLine = string.Empty;
            string[] aryLine = null;
            string[] tableHead = null;
            int columnCount = 0;
            bool isFirst = true;

            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var sr = new StreamReader(fs, Encoding.Default))
                    {
                        while ((strLine = sr.ReadLine()) != null)
                        {
                            if (isFirst)
                            {
                                tableHead = strLine.Split(',');
                                isFirst = false;
                                columnCount = tableHead.Length;

                                for (int i = 0; i < columnCount; i++)
                                {
                                    var tableHeader = tableHead[i].Replace("\"", string.Empty);
                                    DataColumn dc = new DataColumn(tableHeader);
                                    dt.Columns.Add(dc);
                                }
                            }
                            else
                            {
                                aryLine = strLine.Split(',');

                                DataRow dr = dt.NewRow();
                                var val = string.Empty;
                                var col = 0;
                                for (int j = 0; j < aryLine.Length; j++)
                                {
                                    if (aryLine[j].StartsWith("\"") && !aryLine[j].EndsWith("\""))
                                    {
                                        val = aryLine[j];
                                    }
                                    else if (!aryLine[j].StartsWith("\""))
                                    {
                                        val = $"{val},{aryLine[j]}";

                                        if (aryLine[j].EndsWith("\""))
                                        {
                                            dr[col] = val.Trim('\"');
                                            col++;
                                            val = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        dr[col] = aryLine[j].Trim('\"');
                                        col++;
                                    }
                                }

                                dt.Rows.Add(dr);
                            }
                        }

                        if (aryLine != null && aryLine.Length > 0)
                        {
                            dt.DefaultView.Sort = tableHead[0].Replace("\"", string.Empty) + HALF_BLANK + "desc";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return dt;
        }
    }
}
