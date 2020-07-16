using PDCore.Context.IContext;
using PDCore.Extensions;
using PDCore.Interfaces;
using PDCore.Repositories.IRepo;
using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PDCore.Repositories.Repo
{
    public abstract class SqlRepository<T> : SqlRepository, ISqlRepository<T>
    {
        protected SqlRepository(IDbContext db, ILogger logger) : base(db, logger)
        {

        }

        public abstract T FindById(int id);
        public abstract void Add(T newEntity);
        public abstract void AddRange(IEnumerable<T> newEntities);
        public abstract void Delete(T entity);
        public abstract void DeleteRange(IEnumerable<T> entities);
        public abstract IQueryable<T> FindAll();
        public abstract int Commit();
        public abstract List<T> GetByQuery(string query);
        public abstract DataTable GetDataTableByWhere(string where);
        public abstract string GetQuery();

        public string GetTableName()
        {
            string query = GetQuery();

            string tableName = SqlUtils.GetTableName(query);

            return tableName;
        }

        public string GetQuery(string where)
        {
            string query = GetQuery();

            string selection;

            if (query.IndexOf("where", StringComparison.OrdinalIgnoreCase) >= 0)
                selection = " and ";
            else
                selection = " where ";

            selection += where;

            query += selection;

            return query;
        }

        public virtual List<T> GetByWhere(string where)
        {
            string query = GetQuery(where);

            return GetByQuery(query);
        }

        public int GetCountByWhere(string where)
        {
            string tableName = GetTableName();

            string query = SqlUtils.GetCountQuery(tableName, where);

            int count = GetValue<int>(query);

            return count;
        }

        public virtual int GetCount()
        {
            return GetCountByWhere(null);
        }
    }

    public abstract class SqlRepository : ISqlRepository
    {
        protected const string NotSupportedFunctionalityMessage = "Ten typ repozytorium nie oferuje takiej funkcjonalności.";


        private readonly IDbContext db;
        private readonly ILogger logger;

        protected SqlRepository(IDbContext db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;

            if (IsLoggingEnabledByDefault)
                SetLogging(true);
        }

        public bool IsLoggingEnabled => db.IsLoggingEnabled;

        public virtual void SetLogging(bool res)
        {
            db.SetLogging(res, logger);
        }

        public virtual T GetValue<T>(string query)
        {
            DataTable dataTable = GetDataTableByQuery(query);

            T result = dataTable.GetValue<T>();

            return result;
        }

        public abstract DataTable GetDataTableByQuery(string query);

        protected abstract string ConnectionString { get; }

        public static bool IsLoggingEnabledByDefault { get; set; }


        // Flag: Has Dispose already been called?
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //

            db.Dispose();

            disposed = true;
        }

        ~SqlRepository()
        {
            Dispose(false);
        }
    }
}
