using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDWebCore.Context.IContext
{
    public interface IMainDbContext : IEntityFrameworkDbContext, IHasErrorLogDbSet, IHasFileDbSet, IHasUserDataDbSet
    {

    }
}
