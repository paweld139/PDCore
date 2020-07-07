using PDCore.Repositories.IRepo;
using PDCore.Repositories.Repo;
using PDCore.Utils;
using PDWebCore.Context.IContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDWebCore.Repositories.Repo
{
    public class SqlRepositoryEntityFramework<T> : SqlRepository<T>, ISqlRepositoryEntityFramework<T> where T : class
    {
        private readonly IEntityFrameworkDbContext _ctx;
        private readonly DbSet<T> _set;

        public SqlRepositoryEntityFramework(IEntityFrameworkDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
            _set = _ctx.Set<T>();
        }

        public override void Add(T newEntity)
        {
            _set.Add(newEntity);
        }

        public override void AddRange(IEnumerable<T> newEntities)
        {
            _set.AddRange(newEntities);
        }

        public void Attach(T obj)
        {
            _set.Attach(obj);
        }

        public int Commit()
        {
            return _ctx.SaveChanges(); //Zwraca ilość wierszy wziętych po uwagę
        }

        public Task<int> CommitAsync()
        {
            return _ctx.SaveChangesAsync();
        }

        public override void Delete(T entity)
        {
            _set.Remove(entity);
        }

        public override void DeleteRange(IEnumerable<T> entities)
        {
            _set.RemoveRange(entities); ;
        }

        public IQueryable<T> FindAll(bool asNoTracking = true)
        {
            if (asNoTracking)
                return _set.AsNoTracking();

            return _set;
        }

        public override T FindById(int id)
        {
            return _set.Find(id);
        }

        public Task<T> FindByIdAsync(int id)
        {
            return _set.FindAsync(id);
        }

        public Task<List<T>> GetAllAsync(bool asNoTracking = true)
        {
            return FindAll(asNoTracking).ToListAsync();
        }

        private string GetQuery(string where)
        {
            string tableName = _ctx.GetTableName<T>();

            string query = SqlUtils.SQLQuery(tableName, selection: where);

            return query;
        }

        public override List<T> GetByWhere(string where)
        {
            string query = GetQuery(where);

            return _set.SqlQuery(query).ToList();
        }

        public override DataTable GetDataTableByWhere(string where)
        {
            var list = GetByWhere(where);

            return ObjectUtils.CreateDataTable(list);
        }
    }
}
