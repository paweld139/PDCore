using PDCore.Context.IContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Context.IContext
{
    public interface IEntityFrameworkDbContext : IDbContext
    {
        int SaveChanges();

        Task<int> SaveChangesAsync();

        Database Database { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbSet Set(Type entityType);

        DbChangeTracker ChangeTracker { get; }
    }
}
