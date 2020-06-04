using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using PDCore.Interfaces;
using Microsoft.VisualBasic.FileIO;
using PDCore.Extensions;

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

        private static IEnumerable<string> ParseCSV(string filePath, bool skipFirstLine, Func<string, bool> lineCondition)
        {
            IEnumerable<string> lines = File.ReadLines(filePath).Where(x => x.Length > 1);

            if (skipFirstLine)
                lines = lines.Skip(1);

            if (lineCondition != null)
                lines = lines.Where(lineCondition);

            return lines;
        }

        public static List<T> ParseCSV<T>(string filePath, Func<string[], T> fieldsParser, bool skipFirstLine = true, string delimiter = ",", Func<string, bool> lineCondition = null)
        {
            var linesFields = ParseCSVLines(filePath, skipFirstLine, delimiter, lineCondition);

            return linesFields.Select(x => fieldsParser(x)).ToList();
        }

        public static List<T> ParseCSV<T>(string filePath, bool skipFirstLine = true, string delimiter = ",", Func<string, bool> lineCondition = null) where T : IFromCSVParseable, new()
        {
            return ParseCSV(
                filePath,
                x =>
                {
                    var t = new T();
                    t.ParseFromCSV(x);
                    return t;
                }, skipFirstLine, delimiter, lineCondition);
        }

        public static string[] ParseCSVLine(string lineContent, string delimiter = ",")
        {
            using (StringReader stringReader = new StringReader(lineContent))
            {
                using (TextFieldParser textFieldParser = new TextFieldParser(stringReader))
                {
                    textFieldParser.SetDelimiters(delimiter);

                    return textFieldParser.ReadFields();
                }
            }
        }

        public static IEnumerable<string[]> ParseCSVLines(string filePath,  bool skipFirstLine = false, string delimiter = ",", Func<string, bool> lineCondition = null)
        {
            var lines = ParseCSV(filePath, skipFirstLine, lineCondition);

            IEnumerable<string[]> linesFields = lines.Select(x => ParseCSVLine(x, delimiter));

            return linesFields;
        }
    }
}
