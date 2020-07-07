using PDCore.Interfaces;
using PDCore.Repositories.Repo;
using PDWebCore.Context.IContext;
using PDWebCore.Models;
using PDWebCore.Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace PDWebCore.Repositories.Repo
{
    public sealed class LogRepo : SqlRepositoryEntityFramework<LogModel>, ILogRepo
    {
        public LogRepo(IMainDbContext db, IAsyncLogger logger) : base(db, logger) 
        { 

        }
    }
}
