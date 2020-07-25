using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.Comparers
{
    public class KeyValuePairComparer<TKey, TValue> : IComparer<KeyValuePair<TKey, TValue>> where TValue : IComparable<TValue>
    {
        public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        {
            return x.Value.CompareTo(y.Value);
        }
    }
}
