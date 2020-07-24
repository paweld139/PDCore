using PDCore.Extensions;
using PDCore.Interfaces;
using PDCore.Utils;
using PDCoreNew.Context.IContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Extensions
{
    public static class ContextExtensions
    {
        public static ObjectContext GetObjectContext(this DbContext dbContext) => ((IObjectContextAdapter)dbContext).ObjectContext;

        private static DbContext GetDbContext(this IEntityFrameworkDbContext context) => (context as DbContext);


        public static string GetQuery<T>(this ObjectContext context) where T : class
        {
            return context.CreateObjectSet<T>().ToTraceString();
        }

        public static string GetQuery<T>(this DbContext context) where T : class
        {
            return context.GetObjectContext().GetQuery<T>();
        }

        public static string GetQuery<T>(this IEntityFrameworkDbContext context) where T : class
        {
            return context.GetDbContext().GetQuery<T>();
        }

        public static DataTable DataTable(this IEntityFrameworkDbContext context, string query)
        {
            return SqlUtils.GetDataTable(query, context.Database.Connection);
        }

        private static void BeforeSaveChangesWithHistory(IEntityFrameworkDbContext dbContext)
        {
            DateTime dateTime = DateTime.Now;

            foreach (var history in dbContext.ChangeTracker.Entries()
                            .Where(e => e.Entity is IModificationHistory && (e.State == EntityState.Added || e.State == EntityState.Modified))
                            .Select(e => e.Entity as IModificationHistory))
            {
                history.DateModified = dateTime;

                if (history.IsNew())
                    history.DateCreated = dateTime;
            }
        }

        private static void AfterSaveChangesWithHistory(IEntityFrameworkDbContext dbContext)
        {
            foreach (var history in dbContext.ChangeTracker.Entries().OfType<IModificationHistory>())
            {
                history.IsDirty = false;
            }
        }

        public static int SaveChangesWithModificationHistory(this IEntityFrameworkDbContext dbContext)
        {
            BeforeSaveChangesWithHistory(dbContext);

            int result = dbContext.SaveChanges();

            AfterSaveChangesWithHistory(dbContext);

            return result;
        }

        public static async Task<int> SaveChangesWithModificationHistoryAsync(this IEntityFrameworkDbContext dbContext)
        {
            BeforeSaveChangesWithHistory(dbContext);

            int result = await dbContext.SaveChangesAsync();

            AfterSaveChangesWithHistory(dbContext);

            return result;
        }

        public static void SetLogging(this DbContext dbContext, bool input, ILogger logger, bool isLoggingEnabled)
        {
            RepositoryUtils.SetLogging(input, logger, isLoggingEnabled,
                () => dbContext.Database.Log = logger.Info,
                () => dbContext.Database.Log = null
            );
        }

        public static bool IsLoggingEnabled(this DbContext dbContext) => dbContext.Database.Log != null;

        public static bool ExistsLocal<T>(this IEntityFrameworkDbContext dbContext, Func<T, bool> predicate) where T : class
        {
            return dbContext.Set<T>().Local.Any(predicate);
        }

        public static T FirstLocal<T>(this IEntityFrameworkDbContext dbContext, Func<T, bool> predicate) where T : class
        {
            return dbContext.Set<T>().Local.First(predicate);
        }

        public static bool ExistsLocal<T>(this IEntityFrameworkDbContext dbContext, T entity) where T : class
        {
            return dbContext.Set<T>().Local.Contains(entity);
        }

        public static void ConfigureForModificationHistory(this DbModelBuilder modelBuilder)
        {
            modelBuilder.Types().
               Configure(c => c.Ignore("IsDirty"));
        }
    }
}
