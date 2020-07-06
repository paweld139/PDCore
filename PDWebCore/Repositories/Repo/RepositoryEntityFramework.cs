using PDCore.Repositories.Repo;
using PDCore.Utils;
using PDWebCore;
using PDWebCore.Context.IContext;
using PDWebCore.Helpers.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDWebCore.Repositories.Repo
{
    public class RepositoryEntityFramework<T> : Repository<T> where T : class
    {
        private readonly IEntityFrameworkDbContext _db;
        private readonly DbSet<T> _set;
        public RepositoryEntityFramework(IEntityFrameworkDbContext db) : base(db)
        {
            _db = db;
            _set = _db.Set<T>();
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }

        public override DataTable GetDataTable(string where)
        {
            var list = Load(where);

            return ObjectUtils.CreateDataTable(list);
        }

        public override T Load(int id)
        {
            return _set.Find(id);
        }

        public Task<T> LoadAsync(int id)
        {
            return _set.FindAsync(id);
        }

        private string GetQuery(string where)
        {
            string tableName = _db.GetTableName<T>();

            string query = SqlUtils.SQLQuery(tableName, selection: where);

            return query;
        }

        public override List<T> Load(string where)
        {
            string query = GetQuery(where);

            return _set.SqlQuery(query).ToList();
        }

        public override void Save(List<T> list)
        {
            _set.AddRange(list);
        }

        public override void Save(T obj)
        {
            _set.Add(obj);
        }

        public IQueryable<T> Get(bool asNoTracking = true)
        {
            if (asNoTracking)
                return _set.AsNoTracking();

            return _set;
        }

        public Task<List<T>> GetAllAsync(bool asNoTracking = true)
        {
            return Get(asNoTracking).ToListAsync();
        }

        public void Attach(T obj)
        {
            _set.Attach(obj);
        }

        public override void Delete(T obj)
        {
            _set.Remove(obj);
        }

        public override void Delete(List<T> list)
        {
            _set.RemoveRange(list);
        }
    }
}
