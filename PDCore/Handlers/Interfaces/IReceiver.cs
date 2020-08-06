using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Handlers
{
    public interface IReceiver<T> where T : class
    {
        void Handle(T request);
    }
}
