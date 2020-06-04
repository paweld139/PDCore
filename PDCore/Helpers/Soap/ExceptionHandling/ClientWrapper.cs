using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace PDCore.Helpers.Soap.ExceptionHandling
{
    public class ClientWrapper<TProxy, TService> : DisposableWrapper<TProxy> where TProxy : ClientBase<TService> where TService : class
    {
        public ClientWrapper(TProxy proxy) : base(proxy) { }

        protected override void OnDispose()
        {
            // lots of code per state of BaseObject
        }
    }
}
