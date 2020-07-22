﻿using FTCore.CoreLibrary.SQLLibrary;
using Org.BouncyCastle.Asn1.X509.Qualified;
using PDCore.Helpers.Calculation;
using PDCore.Interfaces;
using PDCore.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;

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

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int index = 0;

            foreach (T element in source)
            {
                action(element, index);

                index++;
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

        public static IEnumerable<KeyValuePair<TKey, TValue>> GetKVP<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<int, TValue> valueSelector)
        {
            int i = 0;

            foreach (TSource element in source)
            {
                yield return new KeyValuePair<TKey, TValue>(keySelector(element), valueSelector(i));

                i++;
            }
        }

        public static TResult[] ToArray<TResult>(this IEnumerable<object> source)
        {
            return ToArray<object, TResult>(source);
        }

        public static TResult[] ToArray<TSource, TResult>(this IEnumerable<TSource> source, Converter<TSource, TResult> converter = null)
        {
            return source.ConvertOrCastTo(converter).ToArray();
        }

        public static string[] ToArrayString<T>(this IEnumerable<T> source)
        {
            return source.ToArray(x => x.EmptyIfNull());
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

        public static IEnumerable<TOutput> ConvertOrCastTo<TInput, TOutput>(this IEnumerable<TInput> input, Converter<TInput, TOutput> converter = null)
        {
            return input.Select(x => x.ConvertOrCastTo(converter));
        }

        public static Type GetItemType<T>(this IEnumerable<T> enumerable)
        {
            _ = enumerable;

            return typeof(T);
        }

        public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>(this IDictionary<TKey, TValue> existing)
        {
            return new SortedDictionary<TKey, TValue>(existing);
        }

        public static SortedList<TKey, TValue> ToSortedList<TKey, TValue>(this IDictionary<TKey, TValue> existing)
        {
            return new SortedList<TKey, TValue>(existing);
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
        {
            return new Queue<T>(source);
        }

        public static Stack<T> ToStack<T>(this IEnumerable<T> source)
        {
            return new Stack<T>(source);
        }

        public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> source)
        {
            return new LinkedList<T>(source);
        }

        public static SortedDictionary<TKey, TElement> ToSortedDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return source.ToIDictionary<TSource, TKey, TElement, SortedDictionary<TKey, TElement>>(keySelector, elementSelector);
        }

        public static SortedList<TKey, TElement> ToSortedList<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return source.ToIDictionary<TSource, TKey, TElement, SortedList<TKey, TElement>>(keySelector, elementSelector);
        }

        public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(this IEnumerable<KeyValuePair<TKey, TElement>> source)
        {
            return source.ToIDictionary<TKey, TElement, Dictionary<TKey, TElement>>();
        }

        public static SortedDictionary<TKey, TElement> ToSortedDictionary<TKey, TElement>(this IEnumerable<KeyValuePair<TKey, TElement>> source)
        {
            return source.ToIDictionary<TKey, TElement, SortedDictionary<TKey, TElement>>();
        }

        public static SortedList<TKey, TElement> ToSortedList<TKey, TElement>(this IEnumerable<KeyValuePair<TKey, TElement>> source)
        {
            return source.ToIDictionary<TKey, TElement, SortedList<TKey, TElement>>();
        }

        public static TResult ToIDictionary<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) where TResult : class, IDictionary<TKey, TElement>, new()
        {
            var result = new TResult();

            foreach (var element in source)
            {
                result.Add(keySelector(element), elementSelector(element));
            }

            return result;
        }

        public static TResult ToIDictionary<TKey, TElement, TResult>(this IEnumerable<KeyValuePair<TKey, TElement>> source) where TResult : class, IDictionary<TKey, TElement>, new()
        {
            return source.ToIDictionary<KeyValuePair<TKey, TElement>, TKey, TElement, TResult>(e => e.Key, e => e.Value);
        }

        public static void UpdateValues<TKey, TValue>(this IDictionary<TKey, TValue> source, Func<TValue, TValue> valueSelector) where TKey : ICloneable
        {
            TKey[] keysToUpdate = source.Keys.ToArray();

            keysToUpdate.ForEach(k => source[k] = valueSelector(source[k]));
        }

        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue newValue)
        {
            if (source.ContainsKey(key))
            {
                // yay, value exists!
                source[key] = newValue;
            }
            else
            {
                // darn, lets add the value
                source.Add(key, newValue);
            }
        }

        public static void Update<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue newValue)
        {
            source[key] = newValue;
        }

        public static void Dump<T>(this IEnumerable<T> source, Action<T> print)
        {
            foreach (var item in source)
            {
                print(item);
            }
        }

        public static IEnumerable<TOutput> Map<T, TOutput>(this IEnumerable<T> source, Converter<T, TOutput> converter)
        {
            return source.Select(i => converter(i)); //Mapowanie
        }

        public static IQueryable<T> FindByDate<T>(this IQueryable<T> source, string dateF, string dateT) where T : class, IByDateFindable
        {
            SqlUtils.FindByDate(dateF, dateT, ref source);

            return source;
        }

        public static IQueryable<T> FindByDate<T>(this IQueryable<T> source, DateTime? dateF, DateTime? dateT) where T : class, IByDateFindable
        {
            return source.FindByDate(dateF?.ToString(), dateT?.ToString());
        }

        public static ObjectStatistics<TSource> Aggregate<TSource>(this IEnumerable<TSource> source, Converter<TSource, double> doubleConverter = null)
        {
            return source.Aggregate(new ObjectStatistics<TSource>(),
                                    (acc, i) => acc.Accumulate(i, p => p.ConvertOrCastTo(doubleConverter)),
                                    acc => acc.Compute());
        }

        public static int GetMaxLength(this IEnumerable<string> source)
        {
            return source.Max(s => s.Length);
        }

        public static TOutput[] ConvertArray<TInput, TOutput>(this TInput[] input, Converter<TInput, TOutput> converter)
        {
            return Array.ConvertAll(input, converter);
        }

        public static TOutput[] ConvertOrCastArray<TInput, TOutput>(this TInput[] input, Converter<TInput, TOutput> converter = null)
        {
            return Array.ConvertAll(input, i => i.ConvertOrCastTo(converter));
        }

        public static KeyValuePair<TKey[], TValue[]> ToArrays<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            return new KeyValuePair<TKey[], TValue[]>(keyValuePairs.GetKeys().ToArray(), keyValuePairs.GetValues().ToArray());
        }

        public static KeyValuePair<TKey[], TValue[]> ToArrays<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>[]> keyValuePairs)
        {
            return new KeyValuePair<TKey[], TValue[]>(keyValuePairs.SelectMany(i => i.GetKeys()).ToArray(), keyValuePairs.SelectMany(i => i.GetValues()).ToArray());
        }

        public static IEnumerable<string> StringsThatStartWith(this IEnumerable<string> input, string start)
        {
            foreach (var s in input)
            {
                if (s.StartsWith(start))
                {
                    yield return s;
                }
            }
        }

        public static IEnumerable<string> EmptyIfNull(this IEnumerable<object> values)
        {
            return values.Select(v => v.EmptyIfNull());
        }
    }
}
