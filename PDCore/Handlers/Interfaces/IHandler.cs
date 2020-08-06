using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Handlers
{
    public interface IHandler<T> where T : class
    {
        IHandler<T> SetNext(IHandler<T> next);
        void Handle(T request);
    }
}
