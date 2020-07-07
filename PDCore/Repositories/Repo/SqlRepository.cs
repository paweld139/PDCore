using PDCore.Context.IContext;
using PDCore.Interfaces;
using PDCore.Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PDCore.Repositories.Repo
{
    public abstract class SqlRepository<T> : ISqlRepository<T>
    {
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

        private readonly IDbContext db;
        private readonly ILogger logger;

        public SqlRepository(IDbContext db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public bool IsLoggingEnabled => db.IsLoggingEnabled;

        public virtual void SetLogging(bool res)
        {
            db.SetLogging(res, logger);
        }

        public abstract List<T> GetByWhere(string where);
        public abstract DataTable GetDataTableByWhere(string where);
        public abstract T FindById(int id);
        public abstract void Add(T newEntity);
        public abstract void AddRange(IEnumerable<T> newEntities);
        public abstract void Delete(T entity);
        public abstract void DeleteRange(IEnumerable<T> entities);
    }
}
