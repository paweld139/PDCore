using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using PDCore.Extensions;

namespace PDCore.Utils
{
    public static class SecurityUtils
    {
        public static string Encrypt(string clearText, string encryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);


            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                encryptor.Key = pdb.GetBytes(32);

                encryptor.IV = pdb.GetBytes(16);


                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);

                        cs.Close();
                    }

                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return clearText;
        }

        public static string Encrypt(string clearText)
        {
            return Encrypt(clearText, "AMAR2SPgfhP390");
        }

        public static string Decrypt(string cipherText, string encryptionKey)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);


            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                encryptor.Key = pdb.GetBytes(32);

                encryptor.IV = pdb.GetBytes(16);


                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);

                        cs.Close();
                    }

                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }

        public static string Decrypt(string cipherText)
        {
            return Decrypt(cipherText, "AMARfghPBNRAP390");
        }

        public static string GetUniqueCode(string ident)
        {
            string code = string.Format("{0}{1}{2}", Guid.NewGuid().ToString("N"), ident, DateTime.Now.GetLong());

            code = code.Compress();

            code = code.MD5Hex();

            return code;
        }

        public static string GetUniqueCode(int length)
        {
            StringBuilder builder = new StringBuilder();

            Enumerable
                .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(length)
                .ToList().ForEach(e => builder.Append(e));

            return (builder.ToString());
        }

        /// <summary>
        /// Utworzenie unikalnego identyfikatora żądania
        /// </summary>
        /// <returns>Unikalny identyfikator żądania</returns>
        public static string GetReqId()
        {
            Random rand = new Random();

            long requestId = DateTimeUtils.CurrentTimeMillis() / 1000L;

            requestId += rand.NextLong();


            return requestId.ToString();
        }

        public static string GetGuid(int length)
        {
            if (length > 32 || length < 1)
            {
                return null;
            }

            return Guid.NewGuid().ToString("N").Substring(0, length).ToUpper();
        }
        public static string GetGuid() => Guid.NewGuid().ToString();

        public static List<string> GetUniqueCode(int NumberOfResults, int GuidLength)
        {
            string guid;
            var GuidList = new List<string>();
            var newGuid = new HashSet<string>();

            do
            {
                guid = GetUniqueCode(GuidLength);

                if (newGuid.Add(guid))
                {
                    GuidList.Add(guid);
                }
                else
                {
                    throw new Exception("Ojej");
                }

            } while (GuidList.Count() != NumberOfResults);

            return GuidList;
        }

        /// <summary>
        /// Upewnij się że katalog kończy się '\'
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CorrectDirectoryPath(string path)
        {
            if (path.EndsWith("\\"))
                return path;
            else
                return path + "\\";
        }

        /// <summary>
        /// Ścieżka do katalogu danych tymczasowych
        /// </summary>
        /// <returns></returns>
        public static string TemplateDirPath()
        {
            string path = CorrectDirectoryPath(Environment.GetEnvironmentVariable("TEMP"));

            return path;
        }

        public static Stream GetAssemblyStreamByPath(string path)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            Stream st = assembly.GetManifestResourceStream(path);

            return st;
        }

        public static StreamReader GetAssemblyStreamReaderByPath(string path)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            Stream st = assembly.GetManifestResourceStream(path);

            if (st == null)
            {
                return null;
            }

            StreamReader sr = new StreamReader(st);

            return sr;
        }

        public static X509Certificate2 GetCertificateFromAssembly(string path)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            Stream stream = assembly.GetManifestResourceStream(path);

            if (stream == null)
            {
                return null;
            }

            byte[] certificateBuffer = stream.ReadFully();

            X509Certificate2 x509Certificate2 = new X509Certificate2(certificateBuffer);

            return x509Certificate2;
        }

        public static string GetTempFilePath(string name = null, string extension = null)
        {
            if (string.IsNullOrEmpty(name))
                name = Guid.NewGuid().ToString();

            if (!string.IsNullOrEmpty(extension))
                name = Path.ChangeExtension(name, extension);

            string path = Path.GetTempPath();

            return Path.Combine(path, name);
        }

        public static string SaveTemp(string content, string extension)
        {
            string fileName = GetTempFilePath(extension: extension);

            File.WriteAllText(fileName, content);

            return fileName;
        }

        public static string GetHashSha256(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(text);

            SHA256Managed hashstring = new SHA256Managed();

            byte[] hash = hashstring.ComputeHash(bytes);

            string hashString = string.Empty;

            foreach (byte x in hash)
            {
                hashString += string.Format("{0:x2}", x);
            }

            return hashString;
        }

        public static string Sha512(string input)
        {

            using (var sha = SHA512.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }
    }
}
