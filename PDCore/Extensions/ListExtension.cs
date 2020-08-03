using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace PDCore.Extensions
{
    public static class ListExtension
    {
        public static void RemoveFiles(this List<string> list)
        {
            foreach (string item in list)
            {
                RemoveFile(item);
            }
        }

        private static void RemoveFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void RemoveFiles(this List<KeyValuePair<bool, string>> list)
        {
            list.Where(x => x.Key).ForEach(x => RemoveFile(x.Value));
        }

        public static List<KeyValuePair<TKey, TValue>> GetKVP<TSource, TKey, TValue>(this List<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            List<KeyValuePair<TKey, TValue>> result = new List<KeyValuePair<TKey, TValue>>(source.Count);

            foreach (TSource element in source)
            {
                result.Add(new KeyValuePair<TKey, TValue>(keySelector(element), valueSelector(element)));
            }

            return result;
        }

        public static void AddIfHasValue(this List<string> list, string item)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                list.Add(item);
            }
        }

        public static void AddRangeIfHasValue<T>(this List<T> list, List<T> listToAdd)
        {
            if (listToAdd != null && listToAdd.Count > 0)
            {
                list.AddRange(listToAdd);
            }
        }

        public static IEnumerable<U> GetValues<T, U>(this IEnumerable<KeyValuePair<T, U>> keyValuePairList)
        {
            return keyValuePairList.Select(x => x.Value);
        }

        public static IEnumerable<T> GetKeys<T, U>(this IEnumerable<KeyValuePair<T, U>> keyValuePairList)
        {
            return keyValuePairList.Select(x => x.Key);
        }

        public static IEnumerable<T> Clone<T>(this IEnumerable<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static bool IsEmpty<T>(this ICollection<T> list)
        {
            bool result = list == null || list.Count == 0;

            return result;
        }

        public static T First<T>(this IList<T> list)
        {
            return list[0];
        }

        public static int LastIndex<T>(this IList<T> list)
        {
            int lastIndex = list.Count - 1;

            return lastIndex;
        }

        public static T Last<T>(this IList<T> list)
        {
            return list[list.LastIndex()];
        }

        public static void SetAll<T>(this IList<T> list, T value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = value;
            }
        }
    }
}
