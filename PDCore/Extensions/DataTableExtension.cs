using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PDCore.Extensions
{
    /// <summary>
    /// Statyczna klasa rozszerzająca tabelę danych zawierająca metody pomocne przy operacjach na tejże strukturze danych
    /// </summary>
    public static class DataTableExtension
    {
        /// <summary>
        /// Zwraca informację czy tablica danych posiada wartość
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool HasValue(this DataTable dt)
        {
            //Czy tabela z danymi posiada wiersze, czy pierwsza kolumna pierwszego wiersza nie posiada wartości, czy pierwsza kolumna pierwszego wiersza nie jest pusta (nie jest nullem ani pustym łańcuchem znaków)
            return (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value && !string.IsNullOrWhiteSpace(dt.Rows[0][0].ToString()));
        }

        /// <summary>
        /// Zwraca wartość o zadanym typie z pierwszej kolumny pierwszego wiersza tabeli, jeśli istnieje jakakolwiek wartość
        /// </summary>
        /// <typeparam name="T">Typ wartości jaka ma zostać zwrócona z tabeli</typeparam>
        /// <param name="dt">Tabela z której zostanie pobrana wartość</param>
        /// <returns>Wartość pobrana z tabeli</returns>
        public static T GetValue<T>(this DataTable dt)
        {
            if (!dt.HasValue()) //Jeżeli tabela nie zawiera wartości
            {
                return default(T); //Zwrócona zostaje wartość domyślna dla typu wartości
            }

            return (T)dt.Rows[0][0]; //Zostaje zwrócona wartość pierwszej kolumny z pierwszego wiersza tabeli, która jest jawnie rzutowana na typ wartości
        }

        /// <summary>
        /// Zwraca wartość zadanej kolumny o zadanym typie z pierwszego wiersza tabeli
        /// </summary>
        /// <typeparam name="T">Typ wartości jaka ma zostać zwrócona z tabeli</typeparam>
        /// <param name="dt">Tabela z której zostanie pobrana wartość</param>
        /// <param name="columnName">Nazwa kolumny, której wartość z pierwszego wiersza tabeli ma zostać zwrócona</param>
        /// <returns>Wartość pobrana z tabeli</returns>
        public static T GetValue<T>(this DataTable dt, string columnName)
        {
            if (!dt.HasValue()) //Jeżeli tabela nie zawiera wartości
            {
                return default(T); //Zwrócona zostaje wartość domyślna dla typu wartości
            }

            return (T)dt.Rows[0][columnName]; //Zostaje zwrócona wartość zadanej kolumny z pierwszego wiersza tabeli, która jest jawnie rzutowana na typ wartości
        }

        /// <summary>
        /// Zwraca wartość z pierwszej kolumny pierwszego wiersza tabeli w postaci łańcucha znaków, jeśli istnieje jakakolwiek wartość
        /// </summary>
        /// <param name="dt">Tabela z której zostanie pobrana wartość</param>
        /// <returns>Wartość pobrana z tabeli</returns>
        public static string GetValue(this DataTable dt)
        {
            if (!dt.HasValue()) //Jeżeli tabela nie zawiera wartości
            {
                return string.Empty; //Zwrócony zostaje pusty łańcuch znaków
            }

            return dt.Rows[0][0].ToString(); //Zostaje zwrócona wartość pierwszej kolumny z pierwszego wiersza tabeli, która jest konwertowana na łańcuch znaków
        }

        /// <summary>
        /// Zwraca wartość zadanej kolumny z pierwszego wiersza tabeli w postaci łańcucha znaków
        /// </summary>
        /// <param name="dt">Typ wartości jaka ma zostać zwrócona z tabeli</param>
        /// <param name="columnName">Nazwa kolumny, której wartość z pierwszego wiersza tabeli ma zostać zwrócona</param>
        /// <returns>Wartość pobrana z tabeli</returns>
        public static string GetValue(this DataTable dt, string columnName)
        {
            if (!dt.HasValue()) //Jeżeli tabela nie zawiera wartości
            {
                return string.Empty; //Zwrócony zostaje pusty łańcuch znaków
            }

            return dt.Rows[0][columnName].ToString(); //Zostaje zwrócona wartość zadanej kolumny z pierwszego wiersza tabeli, która jest konwertowana na łańcuch znaków
        }

        /// <summary>
        /// Tworzy listę typu klucz-wartość dla zadanej tabeli
        /// </summary>
        /// <typeparam name="TKey">Typ klucza obiektu klucz-wartość</typeparam>
        /// <typeparam name="TValue">Typ wartości obiektu klucz-wartość</typeparam>
        /// <param name="source">Tabela na podstawie które ma zostać utworzona lista typu klucz-wartość</param>
        /// <param name="keySelector">Metoda, która jako parametr przyjmuje wiersz tabeli i zwraca obiekt typu klucza</param>
        /// <param name="valueSelector">Metoda, która jako parametr przyjmuje wiersz tabeli i zwraca obiekt typu wartości</param>
        /// <returns>Lista typu klucz-wartość dla zadanej tabeli</returns>
        public static List<KeyValuePair<TKey, TValue>> GetKVP<TKey, TValue>(this DataTable source, Func<DataRow, TKey> keySelector, Func<DataRow, TValue> valueSelector)
        {
            //Utworzenie listy typu klucz-wartość z zadanymi typami, która zawiera tyle elementów, co tabela zawiera wierszy (capacity - pojemność)
            List<KeyValuePair<TKey, TValue>> result = new List<KeyValuePair<TKey, TValue>>(source.Rows.Count);

            foreach (DataRow element in source.Rows) //Następuje iteracja po wierszach tabeli
            {
                //Dla każdego wiersza zostaje utworzony obiekt typu klucz-wartość z wykorzystaniem przekazanych metod. Obiekt zostaje dodany do listy.
                result.Add(new KeyValuePair<TKey, TValue>(keySelector(element), valueSelector(element)));
            }

            return result;
        }

        /// <summary>
        /// Tworzy słownik dla zadanej tabeli
        /// </summary>
        /// <typeparam name="TKey">Typ klucza słownika</typeparam>
        /// <typeparam name="TValue">Typ wartości słownika</typeparam>
        /// <param name="source">Tabela na podstawie które ma zostać utworzony słownik</param>
        /// <param name="keySelector">Metoda, które jako parametr przyjmuje wiersz tabeli i zwraca obiekt typu klucza</param>
        /// <param name="valueSelector">Metoda, które jako parametr przyjmuje wiersz tabeli i zwraca obiekt typu wartości</param>
        /// <returns>Słownik dla zadanej tabeli</returns>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this DataTable source, Func<DataRow, TKey> keySelector, Func<DataRow, TValue> valueSelector)
        {
            /*
             * Utworzenie słownika z zadanymi typami, który zawiera tyle elementów, co tabela zawiera wierszy.
             * Słownik wykorzystuje funkcję haszującą, która natychmiastowo przetwarza klucz słownika na odpowiednią wartość.
             * Pozwala to na bardzo szybki dostęp do wartości na podstawie zadanego klucza.
             * W przypadku tablic, dostęp jest realizowany za pomocą indeksu będącego liczbą.
             * Dostęp jest bardzo szybki, ponieważ w pzypadku tablicy, poszczególne elementy
             * znajdują się w pamięci obok siebie i wprost odwołujemy się do ich miejsca w pamięci.
             * Niestety przez to dodawanie, usuwanie czy wstawianie danych do tablicy jest dość złożoną operacją i wymaga przesuwania elementów lub zamieniania ich miejscami.
             * Jest to mozliwe w przypadku struktury ArrayList, która automatycznie wykonuje te operacje.
             * Lista natomiast wykorzystuje tzw. łączniki, czyli każdy element wie jaki jest następny. W przypadku listy, elementy są w różnych miejscach w pamięci.
             * Przez to wybór określonego elementu z listy po indeksie trwa dłużej niż w przypadku tablicy. 
             * Dodawanie, usuwanie czy wstawianie elementów przebiega jednak znacznie szybciej, niż w przypadku tablic.
             * Wystarczy zmienić położenie łączników.
             * Inną kolekcją jest HashSet, który przechowuje wartości określonego typu, które nie mogą się powtarzać. Jesto jakby lista bez duplikatów.
             * Też bardzo szybko następuje znajdywanie obiektów.
             * Stack, czyli stos przechowuje dane w strukturze stosu, czyli każdy następny element ląduje na góre stosu i są ściągane także z góry (LIFO - last-in-first-out).
             * Kolejka przechowuje dane, jak nazwa wskazuje, o strukturze kolejki, czyli każdy element ląduje na końcu, a elementy są popierane od przodu (FIFO first-in-first-out)
             */
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>(source.Rows.Count);

            foreach (DataRow element in source.Rows) //Następuje iteracja po wierszach tabeli
            {
                //Dla każdego wiersza zostaje utworzony wpis słownika z wykorzystaniem przekazanych metod. Wpis zostaje dodany do słownika.
                result.Add(keySelector(element), valueSelector(element));
            }

            return result;
        }
    }
}
