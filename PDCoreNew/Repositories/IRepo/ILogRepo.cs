using PDCore.Repositories.IRepo;
using PDCoreNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Repositories.IRepo
{
    public interface ILogRepo : ISqlRepositoryEntityFramework<LogModel>
    {

    }
}
