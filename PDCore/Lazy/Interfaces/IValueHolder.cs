using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Lazy.Interfaces
{
    public interface IValueHolder<T>
    {
        T GetValue(object parameter);
    }
}
