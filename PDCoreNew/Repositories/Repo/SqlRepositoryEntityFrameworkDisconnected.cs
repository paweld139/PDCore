using PDCore.Interfaces;
using PDCore.Repositories.IRepo;
using PDCoreNew.Context.IContext;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace PDCoreNew.Repositories.Repo
{
    public sealed class SqlRepositoryEntityFrameworkDisconnected<T> :
        SqlRepositoryEntityFrameworkAsync<T>, ISqlRepositoryEntityFrameworkDisconnected<T> where T : class, IModificationHistory
    {
        public SqlRepositoryEntityFrameworkDisconnected(IEntityFrameworkDbContext ctx, ILogger logger) : base(ctx, logger)
        {
        }


        public override IQueryable<T> FindAll()
        {
            return FindAll(true);
        }


        public void SaveNew(T entity)
        {
            Add(entity);

            Commit();
        }

        public Task SaveNewAsync(T entity)
        {
            Add(entity);

            return CommitAsync();
        }

        public void Update(T entity)
        {
            ctx.Entry(entity).State = EntityState.Modified;
        }

        public void SaveUpdated(T entity)
        {
            Update(entity);

            Commit();
        }

        public Task SaveUpdatedAsync(T entity)
        {
            Update(entity);

            return CommitAsync();
        }


        public override void Delete(T entity)
        {
            ctx.Entry(entity).State = EntityState.Deleted;
        }

        public void Delete(int id)
        {
            var entry = FindById(id);

            Delete(entry);
        }


        public void DeleteAndCommit(int id)
        {
            Delete(id);

            Commit();
        }

        public Task DeleteAndCommitAsync(int id)
        {
            Delete(id);

            return CommitAsync();
        }

        public override void DeleteAndCommit(T entity)
        {
            Delete(entity);

            Commit();
        }

        public override Task DeleteAndCommitAsync(T entity)
        {
            Delete(entity);

            return CommitAsync();
        }
    }
}
