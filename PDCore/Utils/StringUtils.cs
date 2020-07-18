using PDCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Utils
{
    public static class StringUtils
    {
        public const string ResultFormat = "{0}:{1}{2}";

        public const string Separator = "***";

        public static string ZeroFix(string element)
        {
            if (element != null && element.Length == 1)
            {
                return "0" + element;
            }

            return element;
        }

        public static string ZeroFixReversed(string element)
        {
            if(element != null && element.Length == 2 && element[0] == '0')
            {
                return element[1].ToString();
            }

            return element;
        }

        public static bool AreNullOrWhiteSpace(params string[] results)
        {
            return AreOrNotNullOrWhiteSpace(true, results);
        }

        public static bool AreNotNullOrWhiteSpace(params string[] results)
        {
            return AreOrNotNullOrWhiteSpace(false, results);
        }

        private static bool AreOrNotNullOrWhiteSpace(bool indicator = false, params string[] results)
        {
            bool result = true;

            foreach (string item in results)
            {
                result &= (string.IsNullOrWhiteSpace(item) == indicator);

                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Zwrócenie szerokości zawartości kolumn na podstawie kolekcji pól wierszy
        /// </summary>
        /// <param name="rowsFields">Kolekcja pól wierszy</param>
        /// <returns>Szerokość zawartości kolumn</returns>
        public static int[] GetColumnsWidths(ICollection<string[]> rowsFields)
        {
            string[] firstRowFields = rowsFields.First(); //Pobranie pól pierwszego elementu kolekcji

            int[] columnsWidths = new int[firstRowFields.Length]; //Tablica zawierająca szerokości kolumn w tabeli, która zostanie wyświetlona. Każde pole jest wyświetlone w określonej kolumnie
            //Szerokości kolumn jest tyle co kolumm. Ilość kolumn została pobrana na podstawie ilości pól z pierwszym elemencie kolekcji. Każdy element powinien mieć taką samą ilość pól.

            foreach (var fields in rowsFields) //Przechodzenie po wszystkich kolekcjach pól wierszy
            {
                fields.ForEach((x, i) => //Przejćie po wszystkich polach celem pobrania i ustalenia najdłuższego pola z danej kolumny
                {
                    if (columnsWidths[i] < x.Length) //Czy aktualnie ustawiona długość kolumny jest mniejsza od długości pola
                        columnsWidths[i] = x.Length; //Ustawienie nowej długości kolumny
                });
            }

            return columnsWidths; //Zwrócenie szerokości zawartości kolumn
        }

        public static KeyValuePair<int, int> GetColumnsWidths(IEnumerable<KeyValuePair<string, string>> rowsFields)
        {
            var rowsFieldsArrays = rowsFields.Select(f => new[] { f.Key, f.Value }).ToList();

            var columnsWidths = GetColumnsWidths(rowsFieldsArrays);


            return new KeyValuePair<int, int>(columnsWidths[0], columnsWidths[1]);
        }
    }
}
