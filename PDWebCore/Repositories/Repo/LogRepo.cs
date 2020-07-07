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
    public class LogRepo : SqlRepositoryEntityFramework<LogModel>
    {
        public LogRepo(IMainDbContext db) : base(db) 
        { 

        }
    }
}
