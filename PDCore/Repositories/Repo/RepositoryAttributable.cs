﻿using FTCore.CoreLibrary.AttributeApi;
using FTCore.CoreLibrary.SQLLibrary;
using PDCore.Context.IContext;
using PDCore.Helpers;
using PDCore.Interfaces;
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
    public class RepositoryAttributable<T> : SqlRepository<T> where T : Attributable, new()
    {
        private readonly IAttributableDbContext db;

        public RepositoryAttributable(IAttributableDbContext db, ILogger logger) : base(db, logger)
        {
            this.db = db;
        }

        public override T FindById(int id)
        {
            return db.Load<T>(id);
        }

        public override List<T> GetByWhere(string where)
        {
            return db.Load<T>(where);
        }

        public override void AddRange(IEnumerable<T> list)
        {
            db.SaveChanges(list);
        }

        public override void Add(T obj)
        {
            db.SaveChanges(obj);
        }

        public override DataTable GetDataTableByWhere(string where)
        {
            return db.GetDataTable(new T(), where);
        }

        public override void Delete(T obj)
        {
            db.Delete(obj);
        }

        public override void DeleteRange(IEnumerable<T> list)
        {
            db.Delete(list);
        }

        public override IQueryable<T> FindAll()
        {
            List<T> entities = new List<T>();

            var result = from entity in entities.AsQueryable() select entity;

            string sql = result.ToString();

            return null;
        }

        public override int Commit()
        {
            throw new NotImplementedException();
        }
    }
}
