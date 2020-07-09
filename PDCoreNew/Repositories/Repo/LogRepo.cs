using PDCore.Interfaces;
using PDCoreNew.Context.IContext;
using PDCoreNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Repositories.Repo
{
    public sealed class LogRepo : SqlRepositoryEntityFramework<LogModel>
    {
        public LogRepo(IEntityFrameworkDbContext ctx) : base(ctx, null)
        {

        }
    }
}
