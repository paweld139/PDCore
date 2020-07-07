﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Repositories.IRepo
{
    public interface ISqlRepositoryEntityFramework<T> : ISqlRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(bool asNoTracking = true);

        IQueryable<T> FindAll(bool asNoTracking = true);

        int Commit();

        Task<int> CommitAsync();

        Task<T> FindByIdAsync(int id);

        void Attach(T obj);
    }
}