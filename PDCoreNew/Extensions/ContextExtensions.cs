using PDCore.Attributes;
using PDCore.Extensions;
using PDCore.Interfaces;
using PDCore.Utils;
using PDCoreNew.Attributes;
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
using System.Reflection;
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
            DateTime dateTime = DateTime.UtcNow;

            IModificationHistory entity;
            EntityState entityState;

            foreach (var history in dbContext.ChangeTracker.Entries()
                            .Where(e => e.Entity is IModificationHistory && (e.State == EntityState.Added || e.State == EntityState.Modified))
                            .Select(e => new { Entity = e.Entity as IModificationHistory, e.State }))
            {
                entity = history.Entity;
                entityState = history.State;

                if (entityState == EntityState.Modified)
                    dbContext.Entry(entity).Property(e => e.DateCreated).IsModified = false;

                if (entityState == EntityState.Added)
                    entity.DateCreated = dateTime;

                entity.DateModified = dateTime;
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

        public static T FirstOrDefautLocal<T>(this IEntityFrameworkDbContext dbContext, Func<T, bool> predicate) where T : class
        {
            return dbContext.Set<T>().Local.FirstOrDefault(predicate);
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

        public static void ConfigureForDateTimeKind(this DbContext context, DateTimeKind? defaultKind = null)
        {
            context.GetObjectContext().ObjectMaterialized += (sender, e) => DateTimeKindAttribute.Apply(e.Entity, defaultKind);
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

                string error;

                foreach (var property in clientEntry.PropertyNames)
                {
                    if (property.ValueIn(nameof(databaseValues.DateModified), nameof(databaseValues.DateCreated)))
                        continue;

                    var databaseValue = databaseEntry[property];

                    var clientValue = clientEntry[property];

                    if (clientValue != databaseValue && !(clientValue?.Equals(databaseValue) ?? false))
                    {
                        error = "Current value: ";

                        if (property == nameof(databaseValues.RowVersion))
                        {
                            error += Convert.ToBase64String(databaseValues.RowVersion);
                        }
                        else
                        {
                            error += databaseValue;
                        }

                        writeError(property, error);
                    }
                }

                writeError(string.Empty, "The record you attempted to edit "
                    + "was modified by another user after you got the original value. The "
                    + "edit operation was canceled and the current values in the database "
                    + "have been displayed. If you still want to edit this record, "
                    + "save again.");

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

        public static void Update<TEntity, TKey>(this DbContext context, IDTO<TKey> dto)
            where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            TEntity entity = context.Set<TEntity>().Local.FirstOrDefault(e => e.Id.Equals(dto.Id));

            if (entity == null)
            {
                entity = context.Set<TEntity>().Create();
                entity.Id = dto.Id;
                context.Set<TEntity>().Attach(entity);
            }

            context.Entry(entity).CurrentValues.SetValues(dto);

            var attribute = dto.GetAttribute<EnsureUpdatedAttribute>();

            if (attribute != null)
            {
                foreach (var property in attribute.Properties)
                    context.Entry(entity).Property(property).IsModified = true;
            }
        }

        public static void EditEntity<TEntity>(this DbContext context,
            TEntity entity,
            TypeOfEditEntityProperty typeOfEditEntityProperty,
            params string[] properties) where TEntity : class
        {
            var find = context.Set<TEntity>().Find(entity.GetType().GetProperty("Id").GetValue(entity, null));

            if (find == null)
                throw new Exception("id not found in database");

            if (typeOfEditEntityProperty == TypeOfEditEntityProperty.Ignore)
            {
                foreach (var item in entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
                {
                    if (!item.CanRead || !item.CanWrite)
                        continue;
                    if (properties.Contains(item.Name))
                        continue;

                    item.SetValue(find, item.GetValue(entity, null), null);
                }
            }
            else if (typeOfEditEntityProperty == TypeOfEditEntityProperty.Take)
            {
                foreach (var item in entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
                {
                    if (!item.CanRead || !item.CanWrite)
                        continue;
                    if (!properties.Contains(item.Name))
                        continue;

                    item.SetValue(find, item.GetValue(entity, null), null);
                }
            }
            else
            {
                foreach (var item in entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
                {
                    if (!item.CanRead || !item.CanWrite)
                        continue;

                    item.SetValue(find, item.GetValue(entity, null), null);
                }
            }

            context.SaveChanges();
        }
    }
}
