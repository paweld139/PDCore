using PDCoreNew.Context.IContext;
using PDCoreNew.Models;
using PDCoreNew.Repositories.IRepo;

namespace PDCoreNew.Repositories.Repo
{
    public sealed class LogRepo : SqlRepositoryEntityFramework<LogModel>, ILogRepo
    {
        public LogRepo(IEntityFrameworkDbContext ctx) : base(ctx, null)
        {

        }
    }
}
