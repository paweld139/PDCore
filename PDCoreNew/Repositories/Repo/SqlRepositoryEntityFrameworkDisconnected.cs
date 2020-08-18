using PDCore.Interfaces;
using PDCore.Repositories.IRepo;
using PDCoreNew.Context.IContext;
using PDCoreNew.Extensions;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace PDCoreNew.Repositories.Repo
{
    public class SqlRepositoryEntityFrameworkDisconnected<T> :
        SqlRepositoryEntityFrameworkAsync<T>, ISqlRepositoryEntityFrameworkDisconnected<T> where T : class, IModificationHistory
    {
        public SqlRepositoryEntityFrameworkDisconnected(IEntityFrameworkDbContext ctx, ILogger logger) : base(ctx, logger)
        {
        }


        public override IQueryable<T> FindAll()
        {
            return FindAll(true);
        }


        public virtual void SaveNew(T entity)
        {
            Add(entity);

            Commit();
        }

        public virtual Task SaveNewAsync(T entity)
        {
            Add(entity);

            return CommitAsync();
        }

        public virtual void SaveUpdated(T entity)
        {
            Update(entity);

            Commit();
        }

        public virtual Task SaveUpdatedAsync(T entity)
        {
            Update(entity);

            return CommitAsync();
        }

        private async Task<bool> DoSaveUpdatedWithOptimisticConcurrency(T entity, Action<string, string> writeError, bool sync)
        {
            Update(entity);

            int rowsAffected = 0;

            try
            {
                if (sync)
                    rowsAffected = Commit();
                else
                    rowsAffected = await CommitAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.HandleExceptionOnEdit(entity, writeError);
            }
            catch (RetryLimitExceededException dex)
            {
                logger.Error("An error occurred while trying to update the entity", dex);

                writeError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return rowsAffected > 0;
        }

        public virtual bool SaveUpdatedWithOptimisticConcurrency(T entity, Action<string, string> writeError)
        {
            return DoSaveUpdatedWithOptimisticConcurrency(entity, writeError, true).Result;
        }

        public virtual Task<bool> SaveUpdatedWithOptimisticConcurrencyAsync(T entity, Action<string, string> writeError)
        {
            return DoSaveUpdatedWithOptimisticConcurrency(entity, writeError, false);
        }


        public override void Delete(T entity)
        {
            ctx.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void Delete(params object[] keyValues)
        {
            var entry = FindByKeyValues(keyValues);

            if (entry == null)
                return; // not found; assume already deleted.

            Delete(entity: entry);
        }


        public virtual void DeleteAndCommit(params object[] keyValues)
        {
            Delete(keyValues);

            Commit();
        }

        public virtual Task DeleteAndCommitAsync(params object[] keyValues)
        {
            Delete(keyValues);

            return CommitAsync();
        }
    }
}
