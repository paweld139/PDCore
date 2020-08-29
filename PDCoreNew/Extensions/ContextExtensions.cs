using PDCore.Extensions;
using PDCore.Interfaces;
using PDCore.Utils;
using PDCoreNew.Context.IContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
            DateTime dateTime = DateTime.UtcNow;

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
            foreach (var history in dbContext.ChangeTracker.Entries()
                                    .Where(e => e.Entity is IModificationHistory)
                                    .Select(e => e.Entity as IModificationHistory))
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
            modelBuilder.Types<IModificationHistory>().
               Configure(c => c.Ignore(e => e.IsDirty));
        }

        public static void HandleExceptionOnEdit<T>(this DbUpdateConcurrencyException exception, T entity, Action<string, string> writeError) where T : class, IModificationHistory
        {
            var entry = exception.Entries.Single();

            var clientEntry = entry.CurrentValues;

            var databaseEntry = entry.GetDatabaseValues();

            if (databaseEntry == null)
            {
                writeError(string.Empty, "Unable to save changes. The object was deleted by another user.");
            }
            else
            {
                var databaseValues = (T)databaseEntry.ToObject();

                foreach (var property in clientEntry.PropertyNames)
                {
                    var databaseValue = databaseEntry[property];

                    var clientValue = clientEntry[property];

                    if (clientValue != databaseValue && !clientValue.Equals(databaseValue))
                    {
                        writeError(property, "Current value: " + databaseValue);
                    }
                }

                writeError(string.Empty, "The record you attempted to edit "
                    + "was modified by another user after you got the original value. The "
                    + "edit operation was canceled and the current values in the database "
                    + "have been displayed. If you still want to edit this record, click "
                    + "the Save button again. Otherwise click the Back to List hyperlink.");

                entity.RowVersion = databaseValues.RowVersion;
            }
        }

        public static IEnumerable<string> GetSchemaDetails<TEntity>(this DbContext context)
        {
            //retrieve object model
            ObjectContext objContext = context.GetObjectContext();

            //retrieve name types
            var nameTypes = objContext.MetadataWorkspace.GetItems<EntityType>(DataSpace.OSpace);

            //set a connection String 
            var connectionString = objContext.Connection.ConnectionString;
            var connection = new EntityConnection(connectionString);
            var workspace = connection.GetMetadataWorkspace();

            var entitySets = workspace.GetItems<EntityContainer>(DataSpace.SSpace).First().BaseEntitySets;

            for (int i = 0; i < nameTypes.Count; i++)
            {
                Type type = Type.GetType(nameTypes[i].FullName);

                yield return entitySets[type.Name].MetadataProperties["Schema"].Value.ToString();
            }
        }
    }
}
