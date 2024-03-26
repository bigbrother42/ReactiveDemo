using System;
using System.Collections.Generic;
using System.IO;
using BaseDemo.Util.Extensions;
using Ionic.Zip;

namespace BaseDemo.Util
{
    public class ZipUtil
    {
        public static void ZipFile(List<string> filePathList, string zipFilePath)
        {
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    foreach (var filePath in filePathList)
                    {
                        zip.AddDirectory(filePath);
                    }

                    zip.Save(zipFilePath);
                }
            }
            catch (Exception e)
            {
                LogUtil.Instance.Error("Zip file failed when exporting!", e);
            }
        }

        private static int LastIndexOfNthStr(string sourceStr, string str, int n)
        {
            int index = -1;
            while (n-- > 0)
            {
                index = sourceStr.LastIndexOf(str, index);
                if (index == -1)
                {
                    break;
                }
            }

            return index;
        }

        public static bool IsZipFile(string filePath)
        {
            var extension = Path.GetExtension(filePath);

            return extension.Equals(".zip", StringComparison.OrdinalIgnoreCase)
                    || extension.Equals(".rar", StringComparison.OrdinalIgnoreCase)
                    || extension.Equals(".7z", StringComparison.OrdinalIgnoreCase)
                    || extension.Equals(".tar", StringComparison.OrdinalIgnoreCase)
                    || extension.Equals(".gz", StringComparison.OrdinalIgnoreCase);
        }
    }
}
