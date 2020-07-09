using PDCore.Utils;
using PDCoreTest.Factory.Factories;
using PDCoreTest.Factory.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreTest.Factory
{
    public class AirConditioner
    {
        private readonly Dictionary<Actions, AirConditionerFactory> _factories;

        public AirConditioner()
        {
            _factories = new Dictionary<Actions, AirConditionerFactory>();

            string factoriesNamespace = typeof(AirConditionerFactory).Namespace;

            foreach (Actions action in ObjectUtils.GetEnumValues<Actions>())
            {
                var factory = (AirConditionerFactory)Activator.CreateInstance(Type.GetType($"{factoriesNamespace}.{action}Factory"));

                _factories.Add(action, factory);
            }
        }

        public static AirConditioner InitializeFactories() => new AirConditioner();

        public IAirConditioner ExecuteCreation(Actions action, double temperature) => _factories[action].Create(temperature);
    }
}
