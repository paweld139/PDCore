using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using PDCore.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PDCore.Utils
{
    public static class IOUtils
    {
        public static string Unzip(string strFileName, string strExtractTo)
        {
            using (Stream stream = File.OpenRead(strFileName))
            {
                return Unzip(stream, strExtractTo);
            }
        }

        public static string Unzip(Stream stream, string strPathToExtract, bool setDateAndTime = false)
        {
            ZipInputStream s = new ZipInputStream(stream);

            ZipEntry theEntry;

            byte[] data = new byte[2048];

            string fileName = string.Empty;


            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directoryName = Path.GetDirectoryName(theEntry.Name);

                fileName = Path.GetFileName(theEntry.Name);


                Directory.CreateDirectory(Path.Combine(strPathToExtract, directoryName));

                if (!string.IsNullOrEmpty(fileName))
                {
                    using (FileStream streamWriter = File.Create(Path.Combine(strPathToExtract, theEntry.Name)))
                    {
                        int size = 2048;

                        while (size > 0)
                        {
                            size = s.Read(data, 0, data.Length);

                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                        }

                        streamWriter.Close();
                    }

                    if (setDateAndTime)
                    {
                        // Set date and time
                        File.SetCreationTime(Path.Combine(strPathToExtract, theEntry.Name), theEntry.DateTime);

                        File.SetLastAccessTime(Path.Combine(strPathToExtract, theEntry.Name), theEntry.DateTime);

                        File.SetLastWriteTime(Path.Combine(strPathToExtract, theEntry.Name), theEntry.DateTime);
                    }

                    if (fileName.EndsWith(".zip"))
                    {
                        Unzip(Path.Combine(strPathToExtract, theEntry.Name), Path.Combine(strPathToExtract, Path.GetFileNameWithoutExtension(theEntry.Name)));

                        File.Delete(Path.Combine(strPathToExtract, theEntry.Name));
                    }
                }
            }

            return fileName;
        }

        public static IEnumerable<FileInfo> GetLargeFiles(string path, int maxFilesCount)
        {
            var query = from file in new DirectoryInfo(path).GetFiles()
                        orderby file.Length descending
                        select file;

            return query.Take(maxFilesCount);
        }

        public static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";

            string ext = Path.GetExtension(fileName).ToLower();

            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(ext);

            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();

            //string mimeType = Registry.GetValue(@"HKEY_CLASSES_ROOT\.pdf", "Content Type", null) as string;

            return mimeType;
        }

        public static int GetFilesCount(string path, bool allDirectories = false, bool throwIfDirectoryNotExists = false)
        {
            var searchOption = allDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            int filesCount = 0;

            if (Directory.Exists(path) || throwIfDirectoryNotExists)
            {
                filesCount = Directory.GetFiles(path, "*.*", searchOption).Length;
            }

            return filesCount;
        }

        public static IEnumerable<KeyValuePair<string, int>> GetProcessesWithThreads()
        {
            Process[] processes = Process.GetProcesses();

            return processes.GetKVP(p => p.ProcessName, p => p.Threads.Count);
        }

        public static Dictionary<string, int> GetProcessesWithThreadsDictionary()
        {
            return GetProcessesWithThreads().ToDictionary();
        }

        public static SortedDictionary<string, int> GetProcessesWithThreadsSortedDictionary()
        {
            return GetProcessesWithThreads().ToSortedDictionary();
        }

        public static SortedList<string, int> GetProcessesWithThreadsSortedList()
        {
            return GetProcessesWithThreads().ToSortedList();
        }

        public static void ToggleConfigEncryption(string sectionName = "connectionStrings")
        {
            // Takes the executable file name without the
            // .config extension.

            // Open the configuration file and retrieve
            // the connectionStrings section.
            Configuration config = ConfigurationManager.OpenExeConfiguration(AppDomain.CurrentDomain.BaseDirectory);

            ConfigurationSection section = config.GetSection(sectionName);


            if (section.SectionInformation.IsProtected)
            {
                // Remove encryption.
                section.SectionInformation.UnprotectSection();
            }
            else
            {
                // Encrypt the section.
                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
            }

            // Save the current configuration.
            config.Save();
        }
    }
}
