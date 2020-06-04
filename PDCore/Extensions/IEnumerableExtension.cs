using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Extensions
{
    /// <summary>
    /// Statyczna klasa rozszerzająca zakres operacji oferowanych przez klasę Enumerable
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Wykrywa duplikaty według zadanego warunku i je usuwa. Warunek określa, które wartości mają być unikalne
        /// </summary>
        /// <typeparam name="TSource">Typ źródłowego modułu wyliczającego. Nie trzeba go podawać, bo jest wnioskowany z typu przekazywanego parametru</typeparam>
        /// <typeparam name="TKey">Typ dla którego został utworzony obiekt IEnumerable</typeparam>
        /// <param name="source">Źródłowy moduł wyliczający</param>
        /// <param name="keySelector">Metoda, która jako parametr przyjmuje typ elementu kolekcji i zwraca obiekt typu wg którego wartości w kolekcji mają być unikalne</param>
        /// <returns>Zwraca obiekt IEnumerable który będzie iterował się po kolekcji pomijając duplikaty, wg wartości które mają być unikalne</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>(); //Utworzenie zbioru korzystającego z funkcji skrótu o zadanym typie

            foreach (TSource element in source) //Następuje iteracja po źródłowym module wyliczającym, który przechodzi po każdym elemencie kolekcji
            {
                /*
                 * Zostaje pobrana wartość, z wykorzystaniem przekazanej metody, wg której elementy kolekcji mają być unikalne. 
                 * Następuje dodanie elementu do utworzonego HashSet, jeśli brak jest drugiego takiego samego. W przeciwnym wypadku
                 * metoda do dodawania zwraca "fałsz". Jeśli element został dodany do HashSet, 
                 * to zostaje zwrócony z kolekcji i brany pod uwagę w ewentualnych następnych operacjach.
                 */
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Wywołuje określoną akcję dla każdego elementu kolekcji, po której elementach nestępuje iteracja
        /// </summary>
        /// <typeparam name="T">Typ źródłowego modułu wyliczającego. Nie trzeba go podawać, bo jest wnioskowany z typu przekazywanego parametru</typeparam>
        /// <param name="source">Źródłowy moduł wyliczający</param>
        /// <param name="action">Metoda, która jako parametr przyjmuje element kolekcji i nic nie zwraca</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source) //Następuje iteracja po źródłowym module wyliczającym, który przechodzi po każdym elemencie kolekcji
            {
                action(element); //Wywołanie przekazanej metody dla elementu
            }
        }

        /// <summary>
        /// Tworzy listę typu klucz-wartość dla zadanego obiektu IEnumerable
        /// </summary>
        /// <typeparam name="TSource">Typ źródłowego modułu wyliczającego. Nie trzeba go podawać, bo jest wnioskowany z typu przekazywanego parametru</typeparam>
        /// <typeparam name="TKey">Typ klucza obiektu klucz-wartość</typeparam>
        /// <typeparam name="TValue">Typ wartości obiektu klucz-wartość</typeparam>
        /// <param name="source">Źródłowy moduł wyliczający</param>
        /// <param name="keySelector">Metoda, która jako parametr przyjmuje element kolekcji i zwraca obiekt typu klucza</param>
        /// <param name="valueSelector">Metoda, która jako parametr przyjmuje element kolekcji i zwraca obiekt typu wartości</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, TValue>> GetKVP<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            foreach (TSource element in source) //Następuje iteracja po źródłowym module wyliczającym, który przechodzi po każdym elemencie kolekcji
            {
                /*
                 * Dla każdego elementu kolekcji zostaje utworzony obiekt typu klucz-wartość z wykorzystaniem przekazanych metod. 
                 * Obiekt zwrócony z kolekcji i brany pod uwagę w ewentualnych następnych operacjach.
                 */
                yield return new KeyValuePair<TKey, TValue>(keySelector(element), valueSelector(element));
            }
        }
    }
}
