﻿using PDCore.Interfaces;
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
        private readonly IEntityFrameworkDbContext ctx;
        private readonly DbSet<T> set;

        public SqlRepositoryEntityFramework(IEntityFrameworkDbContext ctx, ILogger logger) : base(ctx, logger)
        {
            this.ctx = ctx;
            set = this.ctx.Set<T>();
        }

        public override void Add(T newEntity)
        {
            set.Add(newEntity);
        }

        public override void AddRange(IEnumerable<T> newEntities)
        {
            set.AddRange(newEntities);
        }

        public void Attach(T obj)
        {
            set.Attach(obj);
        }

        public int Commit()
        {
            return ctx.SaveChanges(); //Zwraca ilość wierszy wziętych po uwagę
        }

        public Task<int> CommitAsync()
        {
            return ctx.SaveChangesAsync();
        }

        public override void Delete(T entity)
        {
            set.Remove(entity);
        }

        public override void DeleteRange(IEnumerable<T> entities)
        {
            set.RemoveRange(entities); ;
        }

        public IQueryable<T> FindAll(bool asNoTracking = true)
        {
            if (asNoTracking)
                return set.AsNoTracking();

            return set;
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

        private string GetQuery(string where)
        {
            string tableName = ctx.GetTableName<T>();

            string query = SqlUtils.SQLQuery(tableName, selection: where);

            return query;
        }

        public override List<T> GetByWhere(string where)
        {
            string query = GetQuery(where);

            return set.SqlQuery(query).ToList();
        }

        public override DataTable GetDataTableByWhere(string where)
        {
            var list = GetByWhere(where);

            return ObjectUtils.CreateDataTable(list);
        }
    }
}