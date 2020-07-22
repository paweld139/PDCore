using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Builders
{
    public interface IBuilder<T>
    {
        T Build();
    }
}
