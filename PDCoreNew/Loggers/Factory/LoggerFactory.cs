using PDCore.Enums;
using PDCore.Interfaces;
using PDCore.Repositories.IRepo;
using PDCore.Utils;
using PDCoreNew.Context.IContext;
using PDCoreNew.Factories.Fac;
using PDCoreNew.Factories.IFac;
using PDCoreNew.Helpers;
using PDCoreNew.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Loggers.Factory
{
    public class LoggerFactory : Factory<PDCore.Enums.Loggers, ILogger>
    {
        protected override string ElementsNamespace => typeof(Logger).Namespace;

        protected override string ElementsPostfix => "Logger";

        protected override void ConfigureContainer(Container container)
        {
            container.For<ILogMessageFactory>().Use<LogMessageFactory>();
        }

        public static LoggerFactory InitializeLoggers() => new LoggerFactory();
    }

    public abstract class Factory<TEnum, TElement> : IFactory<TEnum, TElement> where TEnum : struct
    {
        private readonly Container container;
        private readonly Dictionary<TEnum, TElement> elements;

        protected Factory()
        {
            container = new Container();

            ConfigureContainer(container);

            elements = new Dictionary<TEnum, TElement>();

            TElement elementTemp;

            foreach (TEnum type in ObjectUtils.GetEnumValues<TEnum>())
            {
                elementTemp = (TElement)container.Resolve(Type.GetType($"{ElementsNamespace}.{type}{ElementsPostfix}"));

                elements.Add(type, elementTemp);
            }
        }

        protected abstract void ConfigureContainer(Container container);

        protected abstract string ElementsNamespace { get; }

        protected abstract string ElementsPostfix { get; }

        public TElement ExecuteCreation(TEnum type) => elements[type];
    }
}
