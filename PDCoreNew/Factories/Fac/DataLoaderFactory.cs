using PDCore.Helpers.DataLoaders;
using PDCore.Interfaces;
using PDCoreNew.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Factories.Fac
{
    public class DataLoaderFactory : Factory<PDCore.Enums.Loaders, IDataLoader>
    {
        protected override string ElementsNamespace => typeof(FileLoader).Namespace;

        protected override string ElementsPostfix => "Loader";

        protected override void ConfigureContainer(Container container)
        {
        }
    }
}
