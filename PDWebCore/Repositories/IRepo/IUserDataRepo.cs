using PDCore.Repositories.IRepo;
using PDWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDWebCore.Repositories.IRepo
{
    public interface IUserDataRepo : ISqlRepositoryEntityFramework<UserDataModel>
    {
        
    }
}
