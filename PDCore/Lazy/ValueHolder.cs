using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Lazy
{
    public interface IValueHolder<T>
    {
        T GetValue(object parameter);
    }

    public class ValueHolder<T> : IValueHolder<T>
    {
        private readonly Func<object, T> getValue;
        private T value;

        public ValueHolder(Func<object, T> getValue)
        {
            this.getValue = getValue;
        }

        public T GetValue(object parameter)
        {
            if (value == null)
            {
                value = getValue(parameter);
            }

            return value;
        }
    }
}
