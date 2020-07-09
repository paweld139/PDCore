using PDCore.Context.IContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Context.IContext
{
    public interface IEntityFrameworkDbContext : IDbContext
    {
        bool ExistsLocal<T>(T entity) where T : class;

        bool ExistsLocal<T>(Func<T, bool> predicate) where T : class;

        T FirstLocal<T>(Func<T, bool> predicate) where T : class;

        int SaveChanges();

        Task<int> SaveChangesAsync();

        Database Database { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbSet Set(Type entityType);
    }
}
