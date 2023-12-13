using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDemo.Util
{
    public class FileUtil
    {
        public static readonly string FILE_EXTENSION = ".csv";
        public static readonly string ENGLISH_COMMA = ",";
        public static readonly string HALF_BLANK = " ";

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
                    using (var sr = new StreamReader(fs, Encoding.UTF8))
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
                                for (int j = 0; j < columnCount; j++)
                                {
                                    dr[j] = aryLine[j].Replace("\"", string.Empty);
                                }

                                dt.Rows.Add(dr);
                            }
                        }

                        if (aryLine != null && aryLine.Length > 0)
                        {
                            dt.DefaultView.Sort = tableHead[0].Replace("\"", string.Empty) + " " + "desc";
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
