using PDCore.Enums;
using PDCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Utils
{
    public static class ConsoleUtils
    {
        #region Write line

        /// <summary>
        /// Wyświetlenia łańcucha znaków w nowej linii
        /// </summary>
        /// <param name="value">Tekst do wyświetlenia</param>
        /// <param name="readKey">Czy po wyświetleniu tekstu nowej linii, oczekiwać na wciśnięcie klawisza</param>
        public static void WriteLine(string value, bool readKey = true)
        {
            Console.WriteLine(value); //Wyświetlenie tekstu w nowej linii

            if (readKey) //Czy czekać na wciśnięcie klawisza
                Console.ReadKey(); //Oczekiwanie na wciśnięcie klawisza
        }

        /// <summary>
        /// Wyświetlenia łańcucha znaków w nowej linii
        /// </summary>
        /// <param name="value">StringBuilder z którego zostanie pozyskany łańcuch znaków do wyświetlenia w nowej linii</param>
        /// <param name="readKey">Czy po wyświetleniu tekstu nowej linii, oczekiwać na wciśnięcie klawisza</param>
        public static void WriteLine(StringBuilder value, bool readKey = true)
        {
            WriteLine(value.ToString(), readKey); //Wyświetlenie tekstu w nowej linii z możliwym oczekiwaniem na wciśnięcie klawisza
        }

        /// <summary>
        /// Wyświetlenie nowej linii
        /// </summary>
        /// <param name="readKey">Czy po wyświetleniu nowej linii, oczekiwać na wciśnięcie klawisza</param>
        public static void WriteLine(bool readKey = true)
        {
            WriteLine(string.Empty, readKey); //Wyświetlenie nowej, pustej linii i oczekiwanie na wciśnięcie klawisza
        }

        /// <summary>
        /// Wyświetlenie na konsoli kolekcji łańcuchów znaków (każdy string w osobnej linii) i na końcu oczekiwanie na wciśnięcie klawisza
        /// </summary>
        /// <param name="value">Kolekcja łańcuchów znaków do wyświetlenia w nowych liniach</param>
        public static void WriteLine(IEnumerable<string> value)
        {
            value.Take(value.Count() - 1).ForEach(x => WriteLine(x, false)); //Wzięcie wszystkich stringów opórcz ostatniego i wyświetlenie każdego w nowej linii

            WriteLine(value.Last()); //Wyśwetlenie ostatniego stringa i oczekiwanie na wciśnięcie klawisza
        }

        /// <summary>
        /// Wyświetlenie na konsoli tablicy łańcuchów znaków (każdy string w osobnej linii) i na końcu oczekiwanie na wciśnięcie klawisza
        /// </summary>
        /// <param name="value">Tablica łańcuchów znaków do wyświetlenia w nowych liniach. Możliwość podawania tekstów po przecinku</param>
        public static void WriteLine(params string[] value)
        {
            WriteLine(value.AsEnumerable()); //Wyświetlenie stringów w nowych liniach i oczekiwanie na wciśnięcie klawisza
        }

        #endregion


        #region Write table

        /// <summary>
        /// Wyświetlenie kolekcji pól wierszy w formie tabeli z nagłówkiem lub bez
        /// </summary>
        /// <param name="rowsFields">Kolekcja pól wierszów</param>
        /// <param name="hasHeader">Czy ma nagłówek. Jeśli tak, to zostanie wyróżniony ramką</param>
        /// <param name="horizontalTextAlignment">Sposób wyrównania tekstu w poziomie, domyślnie jest do lewej</param>
        public static void WriteTableFromFields(IEnumerable<string[]> rowsFields, bool hasHeader = true, HorizontalTextAlignment horizontalTextAlignment = HorizontalTextAlignment.Left)
        {
            if (!rowsFields.Any()) //Czy kolekcja pól wierszy zawiera jakiekolwiek elementy
            {
                WriteLine(); //Wyświetlenie pustej linii

                return; //Wyjście z metody
            }

            string[] firstRowFields = rowsFields.First(); //Pobranie pól pierwszego elementu kolekcji

            int[] columnsWidths = GetColumnsWidths(rowsFields); //Pobranie szerokości zawartości kolumn na podstawie kolekcji pól wierszy


            WriteRowDelimiter(columnsWidths); //Wyświetlenie górnej krawędzi tabeli na o określonej szerokości, na podstawie obliczonych szerokości kolumn


            string firstRow = GetRow(firstRowFields, columnsWidths, horizontalTextAlignment); //Pobranie pól z pierwszego elementu kolekcji i na tej podstawie utworzenie zawartości wiersza biorąc pod uwagę maksymalną szerokość kolumn

            if (hasHeader) //Czy tabela ma posiadać nagłówek
            {
                WriteHeader(firstRow, columnsWidths); //Wyświetlenie nagłówka tabeli na podstawie zawartości pierwszego wiersza tabeli biorąc pod uwagę szerokości kolumn, pod nagłówkiemzostanie dodana krawędź
            }
            else
            {
                WriteRow(firstRow); //Wyświetlenie pierwszego wiersza tabeli
            }

            WriteRows(rowsFields, columnsWidths, horizontalTextAlignment); //Wyświetlenie pozostałych wierszy na podstawie kolekcji pól wierszy biorąc pod uwagę szerokość kolumn
        }

        /// <summary>
        /// Wyświetlenie kolekcji obiektów w formie tabeli z nagłówkiem lub bez
        /// </summary>
        /// <typeparam name="T">Typ obiektu, który ma zostać przetworzony na dany wiersz tabeli. Musi być to typ referencyjny, czyli klasa</typeparam>
        /// <param name="collection">Kolekcja obiektów do przetworzenia</param>
        /// <param name="hasHeader">Czy ma nagłówek. Jeśli tak, to zostanie wyróżniony ramką</param>
        /// <param name="horizontalTextAlignment">Sposób wyrównania tekstu w poziomie, domyślnie jest do lewej</param>
        public static void WriteTableFromObjects<T>(IEnumerable<T> collection, bool hasHeader = true, HorizontalTextAlignment horizontalTextAlignment = HorizontalTextAlignment.Left) where T : class
        {
            IEnumerable<string[]> rowsFields = collection.Select(x => ObjectUtils.GetObjectValues(x).ToArrayString()); //Zwrócenie kolekcji pól dla obiektów
            //Z każdego obiektu zostają pobrane wartości właściwości i zostają przekonwertowane na tablicę łańcuchów znaków - tablicę pól dla danego wiersza

            WriteTableFromFields(rowsFields, hasHeader, horizontalTextAlignment); //Wyświetlenie kolekcji pól w formie tabeli z nagłówkiem lub bez
        }

        /// <summary>
        /// Wyświetlenie na konsoli tabeli z danymi z pliku CSV
        /// </summary>
        /// <param name="filePath">Ścieżka do pliku CSV</param>
        /// <param name="hasHeader">Czy ma nagłówek. Jeśli tak, to zostanie wyróżniony ramką</param>
        /// <param name="skipFirstLine">Czy pominąć pierwszą linię pliku CSV, zazwyczaj jest to nagłówek, a nie zawsze jest potrzebny</param>
        /// <param name="delimiter">Znak oddzielający dane w liniach pliku CSV</param>
        /// <param name="lineCondition">Warunek. który musi spełnić dana linia, by została wzięta pod uwagę</param>
        /// <param name="horizontalTextAlignment">Sposób wyrównania tekstu w poziomie, domyślnie jest do lewej</param>
        public static void WriteTableFromCSV(string filePath, bool hasHeader = true, bool skipFirstLine = false, string delimiter = ",", Func<string, bool> lineCondition = null, HorizontalTextAlignment horizontalTextAlignment = HorizontalTextAlignment.Left)
        {
            IEnumerable<string[]> rowsFields = CSVUtils.ParseCSVLines(filePath, skipFirstLine, delimiter, lineCondition); //Zwrócenie kolekcji pól dla wybranych linii pliku CSV

            WriteTableFromFields(rowsFields, hasHeader, horizontalTextAlignment); //Wyświetlenie kolekcji pól w formie tabeli z nagłówkiem lub bez
        }

        /// <summary>
        /// Zwrócenie szerokości zawartości kolumn na podstawie kolekcji pól wierszy
        /// </summary>
        /// <param name="rowsFields">Kolekcja pól wierszy</param>
        /// <returns>Szerokość zawartości kolumn</returns>
        public static int[] GetColumnsWidths(IEnumerable<string[]> rowsFields)
        {
            string[] firstRowFields = rowsFields.First(); //Pobranie pól pierwszego elementu kolekcji

            int[] columnsWidths = new int[firstRowFields.Length]; //Tablica zawierająca szerokości kolumn w tabeli, która zostanie wyświetlona. Każde pole jest wyświetlone w określonej kolumnie
            //Szerokości kolumn jest tyle co kolumm. Ilość kolumn została pobrana na podstawie ilości pól z pierwszym elemencie kolekcji. Każdy element powinien mieć taką samą ilość pól.

            int index; //Tutaj będzie przetrzymywany tymczasowy indeks pola danego wiersza

            foreach (var fields in rowsFields) //Przechodzenie po wszystkich kolekcjach pól wierszy
            {
                index = 0; //Na początku sprawdzone zostanie pierwsze pole

                fields.ForEach(x => //Przejćie po wszystkich polach celem pobrania i ustalenia najdłuższego pola z danej kolumny
                {
                    if (columnsWidths[index] < x.Length) //Czy aktualnie ustawiona długość kolumny jest mniejsza od długości pola
                        columnsWidths[index] = x.Length; //Ustawienie nowej długości kolumny

                    index++; //Inkrementacja celem przejścia w nastęonym kroku do następnego pola, które będzie się znajdowało w następnej kolumnie
                });
            }

            return columnsWidths; //Zwrócenie szerokości zawartości kolumn
        }

        #endregion


        #region Get and write row

        /// <summary>
        /// Uworzenie zawartości wiersza na podstawie tablicy pól i szerokości kolumn
        /// </summary>
        /// <param name="values">Tablica pól wiersza, pola wylądują w kolumnach</param>
        /// <param name="columnsWidths">Szerokości kolumn do których powędrują pola</param>
        /// <param name="horizontalTextAlignment">Sposób wyrównania tekstu w poziomie, domyślnie jest do lewej</param>
        /// <returns>Zawartość wiersza</returns>
        public static string GetRow(string[] values, int[] columnsWidths, HorizontalTextAlignment horizontalTextAlignment = HorizontalTextAlignment.Left)
        {
            int index = 0; //Będę tu przechowywane tymczasowe indeksy pól dla danego wiersza
            string fieldContent; //Tutaj będzie tymczasowo porzechowywana zawartość danego pola wiersza
            int fieldContentWidth; //Tutaj będzie tymaczasowo przechowywana szerokość zawartości kolumny w której będzie pole
            StringBuilder rowContent = new StringBuilder("|"); //Utworzenie wstępnej zawartości wiersza, lewa krawędź wiersza.

            values.ForEach(x => //Przejście po wszystkich polach
            {
                fieldContentWidth = columnsWidths[index]; //Ustawienie szerokości zawartości danego pola

                fieldContent = horizontalTextAlignment == HorizontalTextAlignment.Left ? x.PadRight(fieldContentWidth) :
                               horizontalTextAlignment == HorizontalTextAlignment.Right ? x.PadLeft(fieldContentWidth) : x.PadBoth(fieldContentWidth);
                //Wybranie odpowiedniej techniki zapełniania wolnego miejsca spacjami w zależności od wybranego sposobu wyrównania tekstu

                rowContent.AppendFormat(" {0} |", fieldContent); //Dodanie do zawartości wiersza zawartość pola, oddzielenie spacją od lewej krawędzi i od prawej oraz dodanie prawej krawędzi pola
                /*
                 * Zostaje pobrana szerokość zawartości kolumny do której powędruje pole. Jeśli pole ma mniejszą długość od zawartości kolumny, to z prawej strony zostaną dodane spacje, by
                 * każda kolumna i wiersz miały takie same szerokości
                 */

                index++; //Inkrementacja celem dodania do wiersza zawartości kolejnego pola
            });

            return rowContent.ToString(); //Zwrócenie zawartości wiersza
        }

        /// <summary>
        /// Wyświetlenie zawartości wiersza bez oczkiwania na wciśnięcie klawisza
        /// </summary>
        /// <param name="content">Zawartość wiersza</param>
        private static void WriteRow(string content)
        {
            WriteLine(content, false); //Wyświetlenie na konsoli zawartości wiersza bez oczkiwania na wciśnięcie klawisza
        }

        /// <summary>
        /// Wyświetlenie nagłówka tabeli na podstawie szerokości kolumn bez oczekiwania na wciśnięcia klawisza i następnie wyświetlenie dolnej krawędzi nagłówka
        /// </summary>
        /// <param name="content">Zawartość nagłówka do wyświetlenia</param>
        /// <param name="columnsWidths">Szerokości kolumn nagłówka</param>
        private static void WriteHeader(string content, int[] columnsWidths)
        {
            WriteLine(content, false); //Wyświetlenie na konsoli zawartości nagłówka bez oczkiwania na wciśnięcie klawisza

            WriteRowDelimiter(columnsWidths); //Wyświetlenie dolnej krawędzi nagłówka biorąc pod uwagę szerokość zawartości kolumn
        }

        /// <summary>
        /// Wyświetlenie krawędzi wiersza oddzielającej go od następnego wiersza
        /// </summary>
        /// <param name="columnsWidths">Szerokość zawartości kolumn</param>
        public static void WriteRowDelimiter(int[] columnsWidths)
        {
            int columnsContentWidth = columnsWidths.Sum();
            int additionalColumnsWidth = columnsWidths.Length * 3 + 1; //Ilość kolumn razy trzy + 1
            int delimiterCharCount =  columnsContentWidth + additionalColumnsWidth;

            IEnumerable<char> delimiterChars = Enumerable.Repeat('-', delimiterCharCount); //Utworzenie kolekcji znaków oddzielających wiersze. Jest to określona ilość minusów.

            string rowDelimiter = string.Concat(delimiterChars); //Zawartość oddzielająca wiersz. Połączenie znaków.

            WriteLine(rowDelimiter, false); //Wyświetlenie krawędzi wiersza oddzielającej go od następnego wiersza bez oczekiwania na wciśnięcie klawisza.
        }
        /// <summary>
        /// Wyświetlenie kolekcji pól wierszy w formie tabeli
        /// </summary>
        /// <param name="rowsFields">Kolekcja pól wierszy</param>
        /// <param name="columnsWidths">Szerokość zawartości kolumn</param>
        /// <param name="horizontalTextAlignment">Sposób wyrównania tekstu w poziomie, domyślnie jest do lewej</param>
        public static void WriteRows(IEnumerable<string[]> rowsFields, int[] columnsWidths, HorizontalTextAlignment horizontalTextAlignment = HorizontalTextAlignment.Left)
        {
            string row; //Tu będzie przechowywana tymaczasowo zawartość danego wiersza

            foreach (var item in rowsFields.Skip(1)) //Przejście po wszystkich kolekcjach pól wierszy pomijając pierwszy wiersz, pierwszy element kolekcji
            {
                row = GetRow(item, columnsWidths, horizontalTextAlignment); //Utworzenie zawartości wiersza na podstawie pól wiersza biorąc pod uwagę szerokość zawartości kolumn

                WriteRow(row); //Wyświetlenie zawartości wiersza bez oczkiwania na wciśnięcie klawisza
            }

            WriteRowDelimiter(columnsWidths); //Wyświetlenie dolnej krawędzi tabeli

            WriteLine(); //Wyświetlenie pustej linii i oczekiwanie na wciśnięcie klawisza
        }

        #endregion
    }
}
