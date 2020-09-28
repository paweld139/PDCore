using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PDCore.Factories.Fac
{
    public abstract class FactoryProvider<TFactory, TIFactory>
    {
        protected static IEnumerable<Type> factories;

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
                   .Where(t => typeof(TIFactory).IsAssignableFrom(t) && !t.IsAbstract);
        }

        public virtual TIFactory CreateFactoryFor(string name)
        {
            var factory = GetFactoryTypeFor(name);

            return (TIFactory)Activator.CreateInstance(factory);
        }

        public virtual Type GetFactoryTypeFor(string name)
        {
            return factories.SingleOrDefault(x => x.Name.ToLowerInvariant().Contains(name.ToLowerInvariant()));
        }

        public virtual IEnumerable<TIFactory> GetAllFactories(params object[] parameters)
        {
            return factories.Select(f => Activator.CreateInstance(f, parameters)).Cast<TIFactory>();
        }

        public abstract TIFactory CreateFactoryFor(params object[] parameters);
    }
}
