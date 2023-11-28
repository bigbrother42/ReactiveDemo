using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BaseDemo.Util
{
    public class EncryptionUtility
    {
        public static string GetEncodedBcpZipHash(string data)
        {
            string str = data + "emmaps1888";
            byte[] input = Encoding.UTF8.GetBytes(str);
            SHA256 sha = new SHA256CryptoServiceProvider();
            byte[] hash = sha.ComputeHash(input);

            StringBuilder buff = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                buff.AppendFormat("{0:X2}", hash[i]);
            }

            return buff.ToString();
        }

        /// <summary>
        /// Encrypts data using AES algorithm
        /// </summary>
        /// <param name="plainText">The orginal text</param>
        /// <param name="Key">The secrety key for the symmetric algorithm</param>
        /// <param name="IV">The initialization vector for the sysmmetric algorithm</param>
        /// <returns></returns>

        public static string EncryptStringByAes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            var encrypted = default(string);

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                if (aesAlg == null)
                    return null;

                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        var encryptedBytes = msEncrypt.ToArray();
                        encrypted = Convert.ToBase64String(encryptedBytes);
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        /// <summary>
        /// Decrypts data using AES algorithm
        /// </summary>
        /// <param name="cipherText">The data that has been encrypted</param>
        /// <param name="Key">The secrety key for the symmetric algorithm</param>
        /// <param name="IV">The initialization vector for the sysmmetric algorithm</param>
        /// <returns></returns>
        public static string DecryptStringByAes(string cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = default(string);

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                if (aesAlg == null)
                    return null;

                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                var cipherTextBytes = Convert.FromBase64String(cipherText);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        /// <summary>
        /// Encrypts a file using AES algorithm
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <param name="Key">The secrety key for the symmetric algorithm</param>
        /// <param name="IV">The initialization vector for the sysmmetric algorithm</param>
        /// <returns></returns>

        public static bool EncryptFile(string filePath, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (filePath.IsNullOrEmpty() || !File.Exists(filePath))
                return false;

            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            try
            {
                // Create an Aes object
                // with the specified key and IV.
                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    // Create an encryptor to perform the stream transform.
                    var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (var mStream = new MemoryStream())
                    {
                        using (var source = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite,
                            FileShare.ReadWrite))
                        {
                            source.CopyTo(mStream);
                        }

                        using (var destination = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                        using (var csEncrypt = new CryptoStream(destination, encryptor, CryptoStreamMode.Write))
                        {
                            mStream.Seek(0, SeekOrigin.Begin);
                            mStream.CopyTo(csEncrypt);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //EmLogUtil.LogException(e);
                return false;
            }

            return true;
        }


        /// <summary>
        /// Encrypts a file to an output path using AES algorithm
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <param name="outputFilePath">The output path of the file</param>
        /// <param name="Key">The secrety key for the symmetric algorithm</param>
        /// <param name="IV">The initialization vector for the sysmmetric algorithm</param>
        /// <param name="isOverwrite">
        ///     if true, overwrite output file if exists.
        ///     The default value is true.
        /// </param>
        /// <returns>
        ///     Return false if <see cref="filePath"/> or <see cref="outputFilePath"/> is not exist,
        ///     or output file is exist and <see cref="isOverwrite"/> is set to false.
        /// </returns>

        public static bool EncryptFile(string filePath, string outputFilePath, byte[] Key, byte[] IV, bool isOverwrite = true)
        {
            // Check arguments.
            if (filePath.IsNullOrEmpty() || !File.Exists(filePath))
                return false;

            if (outputFilePath.IsNullOrWhiteSpace())
                return false;

            if (File.Exists(outputFilePath) && isOverwrite)
                return false;

            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            try
            {
                // Create an Aes object
                // with the specified key and IV.
                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    // Create an encryptor to perform the stream transform.
                    var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (var source = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite,
                        FileShare.ReadWrite))
                    using (var destination = new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    using (var csEncrypt = new CryptoStream(destination, encryptor, CryptoStreamMode.Write))
                    {
                        source.CopyTo(csEncrypt);
                    }
                }
            }
            catch (Exception e)
            {
                //EmLogUtil.LogException(e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Decrypts a file to an output path using AES algorithm
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <param name="outputFilePath">The output path of the file</param>
        /// <param name="Key">The secrety key for the symmetric algorithm</param>
        /// <param name="IV">The initialization vector for the sysmmetric algorithm</param>
        /// <param name="isOverwrite">
        ///     if true, overwrite output file if exists.
        ///     The default value is true.
        /// </param>
        /// <returns>
        ///     Return false if <see cref="filePath"/> or <see cref="outputFilePath"/> is not exist,
        ///     or output file is exist and <see cref="isOverwrite"/> is set to false.
        /// </returns>

        public static bool DecryptFile(string filePath, string outputFilePath, byte[] Key, byte[] IV, bool isOverwrite = true)
        {
            // Check arguments.
            if (filePath.IsNullOrEmpty() || !File.Exists(filePath))
                return false;

            if (outputFilePath.IsNullOrWhiteSpace())
                return false;

            if (File.Exists(outputFilePath) && isOverwrite)
                return false;

            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            try
            {
                // Create an Aes object
                // with the specified key and IV.
                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    // Create an encryptor to perform the stream transform.
                    var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (var source = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    using (var destination = new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    using (var csDecrypt = new CryptoStream(destination, decryptor, CryptoStreamMode.Write))
                    {
                        source.CopyTo(csDecrypt);
                    }
                }
            }
            catch (Exception e)
            {
                //EmLogUtil.LogException(e);
                return false;
            }

            return true;
        }


        /// <summary>
        /// Decrypts a file using AES algorithm
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <param name="Key">The secrety key for the symmetric algorithm</param>
        /// <param name="IV">The initialization vector for the sysmmetric algorithm</param>
        /// <returns></returns>

        public static bool DecryptFile(string filePath, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (filePath.IsNullOrEmpty() || !File.Exists(filePath))
                return false;

            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            try
            {
                // Create an Aes object
                // with the specified key and IV.
                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    // Create an encryptor to perform the stream transform.
                    var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (var source = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                            source.CopyTo(mStream);

                        using (var destination = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                        using (var csDecrypt = new CryptoStream(destination, decryptor, CryptoStreamMode.Write))
                        {
                            mStream.Seek(0, SeekOrigin.Begin);
                            mStream.CopyTo(csDecrypt);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //EmLogUtil.LogException(e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Decrypts a file using AES algorithm
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <param name="Key">The secrety key for the symmetric algorithm</param>
        /// <param name="IV">The initialization vector for the sysmmetric algorithm</param>
        /// <returns>
        ///     Returns an <see cref="MemoryStream"/> used to further proccessing
        /// </returns>
        public static MemoryStream DecryptFileToMemoryStream(string filePath, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (filePath.IsNullOrEmpty() || !File.Exists(filePath))
                return null;

            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            MemoryStream mStream = new MemoryStream();
            Aes aesAlg = null;
            FileStream source = null;
            try
            {
                // Create an Aes object
                // with the specified key and IV.
                aesAlg = Aes.Create();

                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                source = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                var csDecrypt = new CryptoStream(mStream, decryptor, CryptoStreamMode.Write);
                source.CopyTo(csDecrypt);
                csDecrypt.Flush();
                csDecrypt.FlushFinalBlock();
            }
            catch (Exception e)
            {
                //EmLogUtil.LogException(e);
                return mStream;
            }
            finally
            {
                aesAlg?.Dispose();
                source?.Close();
            }

            mStream.Seek(0, SeekOrigin.Begin);

            return mStream;
        }


        /// <summary>
        /// A helper method to create a <see cref="CryptoStream"/> with AES algorithm transformation.
        /// </summary>
        /// <param name="stream">The stream on which to perform the cryptographic transformation.</param>
        /// <param name="transform">The transformation's mode.</param>
        /// <param name="mode">Specifies the mode of a cryptographic stream.</param>
        /// <param name="Key">The secrety key for the symmetric algorithm</param>
        /// <param name="IV">The initialization vector for the sysmmetric algorithm</param>
        /// <returns>
        ///     A <see cref="CryptoStream"/> with AES algorithm transformation.
        /// </returns>
        public static CryptoStream CreateAesCryptoStream(Stream stream, CrptoTransform transform, CryptoStreamMode mode, byte[] Key, byte[] IV)
        {

            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                var iCryptoTransform = transform == CrptoTransform.Encrypt
                    ? aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV)
                    : aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                return new CryptoStream(stream, iCryptoTransform, mode);
            }
        }

        public enum CrptoTransform
        {
            Encrypt,
            Decrypt
        }

        private static readonly byte[] IV = { 12, 21, 43, 17, 57, 35, 67, 27 };
        private static readonly string encryptKey = "orcatest"; // MUST be 8 characters

        public static string EncryptOrcaString(string inputString)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                try
                {
                    byte[] key = Encoding.UTF8.GetBytes(encryptKey);
                    byte[] byteInput = Encoding.UTF8.GetBytes(inputString);
                    DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

                    ICryptoTransform transform = provider.CreateEncryptor(key, IV);
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(byteInput, 0, byteInput.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                }
                catch (Exception ex)
                {
                    //EmLogUtil.Instance.Error(ex);
                }

                return Convert.ToBase64String(memStream.ToArray());
            }
        }

        public static string DecryptOrcaString(string inputString)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                try
                {
                    byte[] key = Encoding.UTF8.GetBytes(encryptKey);
                    byte[] byteInput = new byte[inputString.Length];
                    byteInput = Convert.FromBase64String(inputString);
                    DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

                    ICryptoTransform transform = provider.CreateDecryptor(key, IV);
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(byteInput, 0, byteInput.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                }
                catch (Exception ex)
                {
                    //EmLogUtil.Instance.Error(ex);
                }

                return Encoding.UTF8.GetString(memStream.ToArray());
            }
        }
    }
}
