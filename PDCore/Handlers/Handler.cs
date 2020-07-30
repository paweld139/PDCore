using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Handlers
{
    public abstract class Handler<T> : IHandler<T> where T : class
    {
        private IHandler<T> Next { get; set; }

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

    public class Handler2<T> where T : class
    {
        private readonly IList<IReceiver<T>> receivers;

        public Handler2(params IReceiver<T>[] receivers)
        {
            this.receivers = receivers;
        }

        public virtual void Handle(T request)
        {
            foreach (var receiver in receivers)
            {
                receiver.Handle(request);
            }
        }
    }
}
