using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PDCore.Factories.Fac
{
    public abstract class FactoryProvider<TFactory, TIFactory>
    {
        private static IEnumerable<Type> factories;

        private static bool IsInitialized => factories != null;

        public FactoryProvider()
        {
            if (!IsInitialized)
                InitializeFactories();
        }

        private void InitializeFactories()
        {
            factories = Assembly.GetAssembly(typeof(TFactory))
                   .GetTypes()
                   .Where(t => typeof(TIFactory).IsAssignableFrom(t));
        }

        public virtual TIFactory CreateFactoryFor(string name)
        {
            var factory = factories.Single(x =>
                x.Name.ToLowerInvariant().Contains(name.ToLowerInvariant()));

            return (TIFactory)Activator.CreateInstance(factory);
        }

        public abstract TIFactory CreateFactoryFor(params object[] parameters);
    }
}
