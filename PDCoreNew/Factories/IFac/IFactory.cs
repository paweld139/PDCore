using PDCoreNew.Loggers.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Factories.IFac
{
    public interface IFactory<TEnum, TElement> where TEnum : struct
    {
        TElement ExecuteCreation(TEnum type);
    }
}
