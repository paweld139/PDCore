using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Helpers.DataStructures.Buffer
{
    public class CircularBuffer<T> : Buffer<T>
    {
        private readonly int capacity;

        public CircularBuffer(int capacity = 10)
        {
            this.capacity = capacity;
        }

        public override void Write(T value)
        {
            base.Write(value);

            if (queue.Count > capacity)
            {
                var discard = queue.Dequeue();

                OnItemDiscarded(discard, value);
            }
        }

        private void OnItemDiscarded(T discard, T value)
        {
            if (ItemDiscarded != null)
            {
                var args = new ItemDiscardedEventArgs<T>(discard, value);

                ItemDiscarded(this, args);
            }
        }

        public event EventHandler<ItemDiscardedEventArgs<T>> ItemDiscarded;

        public bool IsFull => queue.Count == capacity;
    }
}
