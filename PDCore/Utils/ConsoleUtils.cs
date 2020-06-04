using PDCore.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PDCore.Utils
{
    public static class ConsoleUtils
    {
        public static void WriteLine(IEnumerable<string> value)
        {
            value.Take(value.Count() - 1).ForEach(x => WriteLine(x, false));

            WriteLine(value.Last());
        }

        public static void WriteLine(params string[] value)
        {
            WriteLine(value.AsEnumerable());
        }

        public static void WriteLine(bool readKey = true)
        {
            WriteLine(string.Empty, readKey);
        }

        public static void WriteLine(string value, bool readKey = true)
        {
            Console.WriteLine(value);

            if (readKey)
                Console.ReadKey();
        }

        public static void WriteLine(StringBuilder value, bool readKey = true)
        {
            WriteLine(value.ToString(), readKey);
        }

        public static void WriteTableFromCSV(string filePath, bool hasHeader = true, bool skipFirstLine = false, string delimiter = ",", Func<string, bool> lineCondition = null)
        {
            var rowsFields = IOUtils.ParseCSVLines(filePath, skipFirstLine, delimiter, lineCondition);

            WriteTable(rowsFields, hasHeader);
        }

        public static void WriteTable(IEnumerable<string[]> rowsFields, bool hasHeader = true)
        {
            if (!rowsFields.Any())
            {
                WriteLine();

                return;
            }

            int[] columnsWidths = new int[rowsFields.First().Length];

            int index;

            foreach (var fields in rowsFields)
            {
                index = 0;

                fields.ForEach(x =>
                {
                    columnsWidths[index] = Math.Max(x.Length, columnsWidths[index]);

                    index++;
                });
            }

            WriteRowDelimiter(columnsWidths);

            string firstRow = GetRow(rowsFields.First(), columnsWidths);

            if (hasHeader)
            {
                WriteHeader(firstRow, columnsWidths);
            }
            else
            {
                WriteRow(firstRow);
            }

            WriteRows(rowsFields, columnsWidths);
        }

        public static void WriteTable<T>(IEnumerable<T> collection, bool hasHeader = true) where T : class
        {
            IEnumerable<string[]> rowsFields = collection.Select(x => ObjectUtils.GetObjectValues(x).ToArray<string>());

            WriteTable(rowsFields, hasHeader);
        }

        private static string GetRow(string[] values, int[] maxWidths)
        {
            int index = 0;
            StringBuilder line = new StringBuilder("|");

            values.ForEach(x =>
            {
                line.AppendFormat(" {0} |", x.PadRight(maxWidths[index]));

                index++;
            });

            return line.ToString();
        }

        private static void WriteRows(IEnumerable<string[]> lines, int[] maxWidths)
        {
            string row;

            foreach (var item in lines.Skip(1))
            {
                row = GetRow(item, maxWidths);

                WriteRow(row);
            }

            WriteRowDelimiter(maxWidths);

            WriteLine();
        }

        private static void WriteRowDelimiter(int[] maxWidths)
        {
            WriteLine(string.Concat(Enumerable.Concat(Enumerable.Repeat(" ", 0), Enumerable.Repeat("-", maxWidths.Sum() + maxWidths.Length * 2 + maxWidths.Length + 1))), false);           
        }

        private static void WriteRow(string content)
        {
            WriteLine(content, false);
        }

        private static void WriteHeader(string content, int[] maxWidths)
        { 
            WriteLine(content, false);

            WriteRowDelimiter(maxWidths);
        }
    }
}
