using PDCore.Extensions;
using PDCore.Helpers.Wrappers;
using PDCore.Interfaces;
using PDCore.Repositories.IRepo;
using PDCore.Repositories.Repo;
using PDCore.Utils;
using PDCoreNew.Context.IContext;
using PDCoreNew.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Repositories.Repo
{
    public class SqlRepositoryEntityFramework<T> : SqlRepository<T>, ISqlRepositoryEntityFramework<T> where T : class, IModificationHistory, new()
    {
        private readonly IEntityFrameworkDbContext ctx;
        private readonly DbSet<T> set;

        protected override string ConnectionString => ctx.Database.Connection.ConnectionString;

        public SqlRepositoryEntityFramework(IEntityFrameworkDbContext ctx, ILogger logger) : base(ctx, logger)
        {
            this.ctx = ctx;
            set = this.ctx.Set<T>();
        }

        public override void Add(T newEntity)
        {
            set.Add(newEntity);
        }

        //Funkcjonalność ConnectedRepository, np. do Bindingu obiektu w WPF.
        public T Add()
        {
            var entry = new T();

            Add(entry);

            return entry;
        }

        public override void AddRange(IEnumerable<T> newEntities)
        {
            set.AddRange(newEntities);
        }

        public void Attach(T obj)
        {
            set.Attach(obj);
        }

        public override int Commit()
        {
            RemoveEmptyEntries();

            return ctx.SaveChangesWithModificationHistory(); //Zwraca ilość wierszy wziętych po uwagę
        }

        public Task<int> CommitAsync()
        {
            RemoveEmptyEntries();

            return ctx.SaveChangesWithModificationHistoryAsync();
        }

        public override void Delete(T entity)
        {
            set.Remove(entity);
        }

        //Funkcjonalność ConnectedRepository. Niekiedy przydaje się w aplikacjach okienkowych, np.by usunąć dany element z grida.
        public void DeleteAndCommit(T entity)
        {
            Delete(entity);

            Commit();
        }

        //Funkcjonalność ConnectedRepository. Niekiedy przydaje się w aplikacjach okienkowych, np.by usunąć dany element z grida.
        public Task DeleteAndCommitAsync(T entity)
        {
            Delete(entity);

            return CommitAsync();
        }

        public override void DeleteRange(IEnumerable<T> entities)
        {
            set.RemoveRange(entities);
        }

        public IQueryable<T> FindAll(bool asNoTracking)
        {
            if (asNoTracking)
                return set.AsNoTracking();

            return set;
        }

        public override IQueryable<T> FindAll()
        {
            return FindAll(true);
        }

        public override T FindById(int id)
        {
            return set.Find(id);
        }

        public Task<T> FindByIdAsync(int id)
        {
            return set.FindAsync(id);
        }

        public Task<List<T>> GetAllAsync(bool asNoTracking = true)
        {
            return FindAll(asNoTracking).ToListAsync();
        }

        public override List<T> GetAll()
        {
            return FindAll().ToList();
        }

        public List<T> GetAll(bool asNoTracking)
        {
            return FindAll(asNoTracking).ToList();
        }

        //Funkcjonalność ConnectedRepository. Nie ma potrzeby pobierania danych wiele razy. Repository i kontekst żyją w danym oknie.
        public ObservableCollection<T> GetAllFromMemory()
        {
            if (set.Local.IsEmpty())
            {
                GetAll();
            }

            return set.Local;
        }


        //Funkcjonalność ConnectedRepository. Nie ma potrzeby pobierania danych wiele razy. Repository i kontekst żyją w danym oknie.
        public async Task<ObservableCollection<T>> GetAllFromMemoryAsync()
        {
            if (set.Local.IsEmpty())
            {
                await GetAllAsync();
            }

            return set.Local;
        }

        //Funkcjonalność ConnectedRepository. Pozbycie się z pamięci przed zapisem obiektów utworzonych, ale niezedytowanych.
        private void RemoveEmptyEntries()
        {
            T entry;

            //you can't remove from or add to a collection in a foreach loop
            for (int i = set.Local.Count; i > 0; i--)
            {
                entry = set.Local[i - 1];

                if (ctx.Entry(entry).State == EntityState.Added && !entry.IsDirty)
                {
                    Delete(entry);
                }
            }
        }

        public override List<T> GetByQuery(string query)
        {
            return set.SqlQuery(query).ToList();
        }

        public Task<List<T>> GetByQueryAsync(string query)
        {
            return set.SqlQuery(query).ToListAsync();
        }

        public Task<List<T>> GetByWhereAsync(string where)
        {
            string query = GetQuery(where);

            return GetByQueryAsync(query);
        }

        public override DataTable GetDataTableByWhere(string where)
        {
            var list = GetByWhere(where);

            return ReflectionUtils.CreateDataTable(list);
        }

        public override DataTable GetDataTableByQuery(string query)
        {
            return DbLogWrapper.Execute(ctx.DataTable, query, ConnectionString, logger, IsLoggingEnabled);
        }

        public Task<int> GetCountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return FindAll().CountAsync(predicate);
            else
                return FindAll().CountAsync();
        }

        public override string GetQuery()
        {
            return ctx.GetQuery<T>();
        }

        public int GetCount(Expression<Func<T, bool>> predicate)
        {
            return FindAll().Count(predicate);
        }

        public override int GetCount()
        {
            return FindAll().Count();
        }
    }
}
