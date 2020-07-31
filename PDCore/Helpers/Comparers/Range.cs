using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.Comparers
{
    public class Range<T> where T : IComparable
    {
        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public bool IsOverlapped(Range<T> other)
        {
            return Min.CompareTo(other.Max) < 0 && other.Min.CompareTo(Max) < 0;
        }

        public T Min { get; }
        public T Max { get; }
    }
}
