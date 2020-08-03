using FTCore.CoreLibrary.AttributeApi;
using PDCore.Context.IContext;
using PDCore.Exceptions;
using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace PDCore.Repositories.Repo
{
    // A base class that implements IDisposable.
    // By implementing IDisposable, you are announcing that
    // instances of this type allocate scarce resources.
    public class SqlRepositoryAttributable<T> : SqlRepository<T> where T : Attributable, new()
    {
        private readonly IAttributableDbContext db;

        protected override string ConnectionString => db.ConnectionString;

        public SqlRepositoryAttributable(IAttributableDbContext db, ILogger logger) : base(db, logger)
        {
            this.db = db;
        }

        public override T FindById(int id)
        {
            return db.Load<T>(id);
        }

        public override List<T> GetByWhere(string where)
        {
            return db.LoadByWhere<T>(where);
        }

        public override void AddRange(IEnumerable<T> list)
        {
            db.SaveChanges(list);
        }

        public override void Add(T obj)
        {
            db.SaveChanges(obj);
        }

        public override void Update(T obj)
        {
            db.SaveChanges(obj);
        }

        public override DataTable GetDataTableByWhere(string where)
        {
            return db.GetDataTable<T>(where);
        }

        public override void Delete(T obj)
        {
            db.Delete(obj);
        }

        public override void DeleteRange(IEnumerable<T> list)
        {
            db.Delete(list);
        }

        public override int Commit()
        {
            throw new NotSupportedFunctionalityException(NotSupportedFunctionalityMessage);
        }

        public override string GetQuery()
        {
            return db.GetQuery<T>();
        }

        public override List<T> GetByQuery(string query)
        {
            return db.LoadByQuery<T>(query);
        }

        public override DataTable GetDataTableByQuery(string query)
        {
            return db.GetDataTable(query);
        }

        public override IEnumerable<T> GetAll()
        {
            return db.Load<T>();
        }

        public override IQueryable<T> FindAll()
        {
            return GetAll().AsQueryable();
        }
    }
}
