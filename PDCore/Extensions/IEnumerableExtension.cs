using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

        public static TResult[] ToArray<TResult>(this IEnumerable<object> source)
        {
            return ToArray<object, TResult>(source);
        }

        public static TResult[] ToArray<TSource, TResult>(this IEnumerable<TSource> source, Converter<TSource, TResult> converter = null)
        {
            return source.ConvertTo(converter).ToArray();
        }

        public static string[] ToArrayString<T>(this IEnumerable<T> source)
        {
            return source.ToArray(x => (x == null ? string.Empty : x.ToString()));
        }

        public static IEnumerable<T> Add<T>(this IEnumerable<T> source, T element, bool addAsFirst = false)
        {
            IEnumerable<T> toAdd = new[] { element };

            if (addAsFirst)
            {
                return toAdd.Concat(source);
            }

            return source.Concat(toAdd);
        }

        public static IEnumerable<TOutput> ConvertTo<TInput, TOutput>(this IEnumerable<TInput> input, Converter<TInput, TOutput> converter = null)
        {
            if (input.GetItemType() is TOutput)
                return input.Cast<TOutput>();

            if (converter != null)
                return input.Select(x => converter(x));
            
            var simpleConverter = TypeDescriptor.GetConverter(typeof(TInput));

            return input.Select(x => (TOutput)simpleConverter.ConvertTo(x, typeof(TOutput)));
        }

        public static Type GetItemType<T>(this IEnumerable<T> enumerable)
        {
            return typeof(T);
        }
    }
}
