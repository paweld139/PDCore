using PDCore.Helpers.Calculation;
using PDCore.Interfaces;
using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

        ///// <summary>
        ///// Tworzy listę typu klucz-wartość dla zadanego obiektu IEnumerable
        ///// </summary>
        ///// <typeparam name="TSource">Typ źródłowego modułu wyliczającego. Nie trzeba go podawać, bo jest wnioskowany z typu przekazywanego parametru</typeparam>
        ///// <typeparam name="TKey">Typ klucza obiektu klucz-wartość</typeparam>
        ///// <typeparam name="TValue">Typ wartości obiektu klucz-wartość</typeparam>
        ///// <param name="source">Źródłowy moduł wyliczający</param>
        ///// <param name="keySelector">Metoda, która jako parametr przyjmuje element kolekcji i zwraca obiekt typu klucza</param>
        ///// <param name="valueSelector">Metoda, która jako parametr przyjmuje element kolekcji i zwraca obiekt typu wartości</param>
        ///// <returns></returns>
        //public static IQueryable<KeyValuePair<TKey, TValue>> GetKVP<TSource, TKey, TValue>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TValue>> valueSelector)
        //{
        //    var keyPropertyName = ReflectionUtils.GetNameOf(keySelector);
        //    var valuePropertyName = ReflectionUtils.GetNameOf(valueSelector);

        //    var itemParam = Expression.Parameter(typeof(TSource), "item");

        //    var itemKeyExpression = Expression.Property(itemParam, keyPropertyName);
        //    var itemValueExpression = Expression.Property(itemParam, valuePropertyName);

        //    var constructor = typeof(KeyValuePair<TKey, TValue>).GetConstructor(types: new[] { typeof(TKey), typeof(TValue) });

        //    var newExpression = Expression.New(constructor, itemKeyExpression, itemValueExpression);

        //    var lambda = Expression.Lambda<Func<TSource, KeyValuePair<TKey, TValue>>>(newExpression, itemParam);


        //    return source.Select(lambda);
        //}

        public static IEnumerable<KeyValuePair<TKey, TValue>> GetKVP<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            return source.Select(e => new KeyValuePair<TKey, TValue>(keySelector(e), valueSelector(e)));
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

        public static List<KeyValuePair<TKey, TValue>> GetKVPList<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            return source.GetKVP(keySelector, valueSelector).ToList();
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

        public static IEnumerable<T> FindByDate<T>(this IEnumerable<T> source, string dateF, string dateT, Expression<Func<T, DateTime>> dateSelector) where T : class
        {
            return source.AsQueryable().FindByDate(dateF, dateT, dateSelector);
        }

        public static IEnumerable<T> FindByDate<T>(this IEnumerable<T> source, DateTime? dateF, DateTime? dateT, Expression<Func<T, DateTime>> dateSelector) where T : class
        {
            return source.AsQueryable().FindByDate(dateF, dateT, dateSelector);
        }

        public static IQueryable<T> FindByDate<T>(this IQueryable<T> source, string dateF, string dateT, Expression<Func<T, DateTime>> dateSelector) where T : class
        {
            SqlUtils.FindByDate(dateF, dateT, dateSelector, ref source);

            return source;
        }

        public static IQueryable<T> FindByDate<T>(this IQueryable<T> source, DateTime? dateF, DateTime? dateT, Expression<Func<T, DateTime>> dateSelector) where T : class
        {
            return source.FindByDate(dateF?.ToString(), dateT?.ToString(), dateSelector);
        }

        public static IQueryable<T> FindByDateCreated<T>(this IQueryable<T> source, string dateF, string dateT) where T : class, IModificationHistory
        {
            return source.FindByDate(dateF, dateT, e => e.DateCreated);
        }

        public static IQueryable<T> FindByDateModified<T>(this IQueryable<T> source, string dateF, string dateT) where T : class, IModificationHistory
        {
            return source.FindByDate(dateF, dateT, e => e.DateModified);
        }

        public static IQueryable<T> FindByDateCreated<T>(this IQueryable<T> source, DateTime? dateF, DateTime? dateT) where T : class, IModificationHistory
        {
            return source.FindByDateCreated(dateF?.ToString(), dateT?.ToString());
        }

        public static IQueryable<T> FindByDateModified<T>(this IQueryable<T> source, DateTime? dateF, DateTime? dateT) where T : class, IModificationHistory
        {
            return source.FindByDateModified(dateF?.ToString(), dateT?.ToString());
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

        public static Expression<Func<string, bool>> StringContains(this string subString)
        {
            MethodInfo contains = typeof(string).GetMethod("Contains");
            ParameterExpression param = Expression.Parameter(typeof(string), "s");
            var call = Expression.Call(param, contains, Expression.Constant(subString, typeof(string)));
            return Expression.Lambda<Func<string, bool>>(call, param);
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> input, string substring, Expression<Func<T, string>> propertySelector)
        {
            var methodInfo = typeof(string).GetMethod("Contains");

            string propertyName = ReflectionUtils.GetNameOf(propertySelector);

            var itemParam = Expression.Parameter(typeof(T), "item");
            var itemPropertyExpr = Expression.Property(itemParam, propertyName);
            var substringConstant = Expression.Constant(substring);
            //var methodConstant = Expression.Constant(StringComparison.OrdinalIgnoreCase);

            var body = Expression.Call(itemPropertyExpr, methodInfo, substringConstant);

            var lambda = Expression.Lambda<Func<T, bool>>(body, itemParam);


            return input.Where(lambda);
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> input, string substring, Expression<Func<T, string>> propertySelector)
        {
            return input.AsQueryable().Filter(substring, propertySelector);
        }

        public static IEnumerable<string> EmptyIfNull(this IEnumerable<object> values)
        {
            return values.Select(v => v.EmptyIfNull());
        }

        public static IEnumerable<T> GetPage<T>(this IEnumerable<T> input, int page, int pagesize)
        {
            return input.AsQueryable().GetPage(page, pagesize);
        }

        public static IQueryable<T> GetPage<T>(this IQueryable<T> input, int page, int pagesize)
        {
            int elementsToSkip = (page - 1) * pagesize;

            return input.Skip(elementsToSkip).Take(pagesize);
        }

        public static IEnumerable<string> GetSentences(this IEnumerable<string> source)
        {
            return source.SelectMany(s => s.GetSentences());
        }

        public static T[] Concat<T>(this T[] x, T[] y)
        {
            if (x == null)
                throw new ArgumentNullException("x");

            if (y == null)
                throw new ArgumentNullException("y");

            int oldLen = x.Length;

            Array.Resize(ref x, x.Length + y.Length);
            Array.Copy(y, 0, x, oldLen, y.Length);

            return x;
        }

        public static T[] Concat<T>(this T[] x, T y)
        {
            if (x == null)
                throw new ArgumentNullException("x");

            if (y == null)
                throw new ArgumentNullException("y");

            int index = x.Push(y);

            if (index == -1)
            {
                Array.Resize(ref x, x.Length + 1);

                x = x.Concat(y);
            }

            return x;
        }

        public static int Push<T>(this T[] source, T value)
        {
            var index = Array.IndexOf(source, default);

            if (index != -1)
            {
                source[index] = value;
            }

            return index;
        }
    }
}
