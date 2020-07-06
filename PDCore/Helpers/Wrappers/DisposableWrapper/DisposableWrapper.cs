using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.Wrappers.DisposableWrapper
{
    public interface IDisposableWrapper<T> : IDisposable
    {
        T BaseObject { get; }
    }

    public class DisposableWrapper<T> : IDisposableWrapper<T> where T : class, IDisposable
    {
        public T BaseObject { get; private set; }

        public DisposableWrapper(T baseObject) { BaseObject = baseObject; }

        protected virtual void OnDispose()
        {
            BaseObject.Dispose();
        }

        public void Dispose()
        {
            if (BaseObject != null)
            {
                try
                {
                    OnDispose();
                }
                catch { } // swallow...
            }

            BaseObject = null;
        }
    }
}
