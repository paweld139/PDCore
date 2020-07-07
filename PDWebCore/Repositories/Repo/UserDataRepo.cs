using PDCore.Interfaces;
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
    public sealed class UserDataRepo : SqlRepositoryEntityFramework<UserDataModel>, IUserDataRepo
    {
        public UserDataRepo(IMainDbContext db, IAsyncLogger logger) : base(db, logger)
        {

        }
    }
}
