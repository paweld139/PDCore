using AutoMapper;
using PDCore.Interfaces;
using PDCore.Models;
using PDCore.Repositories.IRepo;
using PDCoreNew.Context.IContext;
using PDCoreNew.Extensions;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PDCoreNew.Repositories.Repo
{
    public class SqlRepositoryEntityFrameworkDisconnected<T> :
        SqlRepositoryEntityFrameworkAsync<T>, ISqlRepositoryEntityFrameworkDisconnected<T> where T : class, IModificationHistory
    {
        public SqlRepositoryEntityFrameworkDisconnected(IEntityFrameworkDbContext ctx, ILogger logger, IMapper mapper) : base(ctx, logger, mapper)
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

        public async virtual Task SaveNewAsync<TInput>(TInput input)
        {
            var entity = mapper.Map<T>(input);

            await SaveNewAsync(entity);

            mapper.Map(entity, input);
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

        private async Task<bool> DoSaveUpdatedWithOptimisticConcurrency(T entity, Action<string, string> writeError, bool sync, bool update, bool? include, params Expression<Func<T, object>>[] properties)
        {
            if (update)
            {
                if (include == null)
                {
                    Update(entity);
                }
                else
                {
                    UpdateWithIncludeOrExcludeProperties(entity, include.Value, properties);
                }
            }

            bool result = false;

            try
            {
                if (sync)
                    Commit();
                else
                    await CommitAsync();

                result = true;
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

            return result;
        }

        public virtual bool SaveUpdatedWithOptimisticConcurrency(T entity, Action<string, string> writeError, bool update, bool? include = null, params Expression<Func<T, object>>[] properties)
        {
            return DoSaveUpdatedWithOptimisticConcurrency(entity, writeError, true, update, include, properties).Result;
        }

        public virtual Task<bool> SaveUpdatedWithOptimisticConcurrencyAsync(T entity, Action<string, string> writeError, bool update, bool? include = null, params Expression<Func<T, object>>[] properties)
        {
            return DoSaveUpdatedWithOptimisticConcurrency(entity, writeError, false, update, include, properties);
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

        public virtual void Update(T entity, IHasRowVersion dto)
        {
            var entry = ctx.Entry(entity);

            entry.Property(e => e.RowVersion).OriginalValue = dto.RowVersion;

            entry.CurrentValues.SetValues(dto);
        }
    }
}
