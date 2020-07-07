using PDWebCore.Context.IContext;
using PDWebCore.Models;
using PDWebCore.Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDWebCore.Repositories.Repo
{
    public class UserDataRepo : SqlRepositoryEntityFramework<UserDataModel>
    {
        public UserDataRepo(IMainDbContext db) : base(db)
        {

        }
    }
}
