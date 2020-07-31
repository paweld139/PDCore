using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Factories.Fac
{
    public abstract class Factory<T>
    {
        public abstract T Create(params object[] parameters);

        public virtual T Get(params object[] parameters)
        {
            var provider = Create(parameters);

            return provider;
        }
    }
}
