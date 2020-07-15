using PDCore.Utils;
using PDCoreNew.Context.IContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Extensions
{
    public static class ContextExtensions
    {
        public static string GetQuery<T>(this ObjectContext context) where T : class
        {
            string query = context.CreateObjectSet<T>().ToTraceString();

            return query;
        }

        public static string GetQuery<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

            return objectContext.GetQuery<T>();
        }

        public static string GetQuery<T>(this IEntityFrameworkDbContext context) where T : class
        {
            return (context as DbContext).GetQuery<T>();
        }

        //public static string GetTableName<T>(this DbContext context) where T : class
        //{
        //    ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

        //    return objectContext.GetTableName<T>();
        //}

        //public static string GetTableName<T>(this IEntityFrameworkDbContext context) where T : class
        //{
        //    return (context as DbContext).GetTableName<T>();
        //}

        //public static string GetTableName<T>(this ObjectContext context) where T : class
        //{
        //    string sql = context.CreateObjectSet<T>().ToTraceString();

        //    string tableName = SqlUtils.GetTableName(sql);

        //    return tableName;
        //}

        //public static string GetQuery<T>(this IEntityFrameworkDbContext context, string where) where T : class
        //{
        //    string tableName = context.GetTableName<T>();

        //    string query = SqlUtils.SQLQuery(tableName, selection: where);

        //    return query;
        //}

        public static DataTable DataTable(this IEntityFrameworkDbContext context, string query)
        {
            return SqlUtils.GetDataTable(query, context.Database.Connection);
        }
    }
}
