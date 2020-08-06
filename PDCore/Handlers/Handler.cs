using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Handlers
{
    public class Handler<T> : IHandler<T> where T : class
    {
        protected IHandler<T> Next { get; set; }

        public virtual void Handle(T request)
        {
            Next?.Handle(request);
        }

        public IHandler<T> SetNext(IHandler<T> next)
        {
            Next = next;

            return Next;
        }
    }
}
