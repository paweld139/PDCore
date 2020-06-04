using PDCore.Context.IContext;
using PDCore.Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PDCore.Repositories.Repo
{
    // A base class that implements IDisposable.
    // By implementing IDisposable, you are announcing that
    // instances of this type allocate scarce resources.
    public abstract class Repository<T> : IDisposable where T : class
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

            _db.Dispose();

            disposed = true;
        }

        ~Repository()
        {
            Dispose(false);
        }

        private readonly IDbContext _db;
        public Repository(IDbContext db)
        {
            _db = db;
        }

        public virtual bool IsLoggingEnabled
        {
            get { return _db.IsLoggingEnabled; }
        }

        public virtual void SetLogging(bool res)
        {
            _db.SetLogging(res);
        }

        public abstract void Save(List<T> list);

        public abstract void Save(T obj);

        public abstract T Load(int id);

        public abstract List<T> Load(string where);

        public abstract DataTable GetDataTable(string where);

        public abstract void Delete(T obj);

        public abstract void Delete(List<T> list);
    }
}
