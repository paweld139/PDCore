using PDCore.Extensions;
using PDCore.Factories.IFac;
using PDCore.Utils;
using PDCoreNew.Helpers;
using System;
using System.Collections.Generic;

namespace PDCoreNew.Factories.Fac
{
    public abstract class Factory<TEnum, TElement> : IFactory<TEnum, TElement> where TEnum : struct
    {
        private static Container container;
        private static Dictionary<TEnum, TElement> elements;

        protected Factory()
        {
            if (!IsInitialized)
                InitializeFactory();
        }

        private static bool IsInitialized => elements != null;

        private void InitializeContainer()
        {
            container = new Container();

            ConfigureContainer(container);
        }

        private void InitializeElements()
        {
            elements = new Dictionary<TEnum, TElement>();

            TElement elementTemp;

            foreach (TEnum type in EnumUtils.GetEnumValues<TEnum>())
            {
                elementTemp = (TElement)container.Resolve(Type.GetType($"{ElementsNamespace}.{type}{ElementsPostfix}"));

                elements.Add(type, elementTemp);
            }
        }

        private void InitializeFactory()
        {
            InitializeContainer();

            InitializeElements();
        }

        protected abstract void ConfigureContainer(Container container);

        protected abstract string ElementsNamespace { get; }

        protected abstract string ElementsPostfix { get; }

        public TElement ExecuteCreation(TEnum type) => elements[type];

        public IEnumerable<TElement> GetElements()
        {
            return elements.GetValues();
        }
    }
}
