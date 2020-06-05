using ICSharpCode.SharpZipLib.Zip;
using System.IO;

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
    }
}
