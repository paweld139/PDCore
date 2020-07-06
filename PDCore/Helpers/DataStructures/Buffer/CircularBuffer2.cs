using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Helpers.DataStructures.Buffer
{
    public class CircularBuffer2<T>
    {
        private readonly T[] buffer;
        private int start;
        private int end;

        public CircularBuffer2(int capacity = 10)
        {
            buffer = new T[capacity + 1];
            start = 0;
            end = 0;
        }

        public void Write(T value)
        {
            buffer[end] = value;
            end = (end + 1) % buffer.Length;

            if (end == start)
            {
                start = (start + 1) % buffer.Length;
            }
        }

        public T Read()
        {
            T result = buffer[start];
            start = (start + 1) % buffer.Length;

            return result;
        }

        public int Capacity => buffer.Length;

        public bool IsEmpty => end == start;

        public bool IsFull => (end + 1) % buffer.Length == start;
    }
}
