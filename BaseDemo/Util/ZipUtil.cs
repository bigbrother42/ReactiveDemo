using BaseDemo.Util.Extensions;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDemo.Util
{
    public class ZipUtil
    {
        public const string DOUBLE_SLASH_RIGHT = "//";

        public static void ZipFile(List<string> filePathList, string zipFilePath)
        {
            using (ZipOutputStream outstream = new ZipOutputStream(File.Create(zipFilePath)))
            {
                outstream.SetLevel(6);
                ZipCompress(filePathList, outstream, zipFilePath);
            }

            foreach (var filePath in filePathList)
            {
                File.Delete(filePath);
            }
        }

        public static void ZipCompress(List<string> filePathList, ZipOutputStream outstream, string staticFile)
        {
            Crc32 crc = new Crc32();

            foreach (string file in filePathList)
            {
                using (FileStream fs = File.OpenRead(file))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempfile);
                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    outstream.PutNextEntry(entry);

                    outstream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        public static bool UnZipFile(string zipFilePath, string unZipDir, string applicationImagePath, out string err)
        {
            err = string.Empty;
            if (zipFilePath == string.Empty)
            {
                err = "File can not be empty！";
                return false;
            }

            if (!File.Exists(zipFilePath))
            {
                err = "File is not exist！";
                return false;
            }

            if (unZipDir.IsNullOrEmpty())
            {
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            }

            if (!unZipDir.EndsWith(DOUBLE_SLASH_RIGHT))
            {
                unZipDir += DOUBLE_SLASH_RIGHT;
            }

            if (!Directory.Exists(unZipDir))
            {
                Directory.CreateDirectory(unZipDir);
            }
            else
            {
                // if target unzip directory has import files, then clear
                var fileList = Directory.GetFiles(unZipDir);
                foreach (var file in fileList)
                {
                    File.Delete(file);
                }
            }

            try
            {
                using (ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = zipInputStream.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);

                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(unZipDir + directoryName);
                        }

                        if (!directoryName.EndsWith(DOUBLE_SLASH_RIGHT))
                        {
                            directoryName += DOUBLE_SLASH_RIGHT;
                        }

                        if (string.Equals(Path.GetExtension(fileName), ".csv"))
                        {
                            if (!fileName.IsNullOrEmpty())
                            {
                                using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                                {
                                    int size = 2048;
                                    byte[] data = new byte[2048];
                                    while (true)
                                    {
                                        size = zipInputStream.Read(data, 0, data.Length);
                                        if (size > 0)
                                        {
                                            streamWriter.Write(data, 0, size);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            File.Copy(theEntry.Name, applicationImagePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return false;
            }
            return true;
        }
    }
}
