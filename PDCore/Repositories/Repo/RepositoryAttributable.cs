using FTCore.CoreLibrary.AttributeApi;
using PDCore.Context.IContext;
using PDCore.Helpers;
using PDCore.Utils;
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
    public class RepositoryAttributable<T> : Repository<T> where T : Attributable, new()
    {
        private readonly IAttributableDbContext db;
        public RepositoryAttributable(IAttributableDbContext db) : base(db)
        {
            this.db = db;
        }

        public override T Load(int id)
        {
            return db.Load<T>(id);
        }

        public override List<T> Load(string where)
        {
            return db.Load<T>(where);
        }

        public override void Save(List<T> list)
        {
            db.SaveChanges(list);
        }

        public override void Save(T obj)
        {
            db.SaveChanges(obj);
        }

        public override DataTable GetDataTable(string where)
        {
            return db.GetDataTable(new T(), where);
        }

        public override void Delete(T obj)
        {
            db.Delete(obj);
        }

        public override void Delete(List<T> list)
        {
            db.Delete(list);
        }
    }
}
