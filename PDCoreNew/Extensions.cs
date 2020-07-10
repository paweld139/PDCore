using PDCore.Helpers.Wrappers.DisposableWrapper;
using PDCore.Utils;
using PDCoreNew.Context.IContext;
using PDCoreNew.Helpers;
using PDCoreNew.Repositories.IRepo;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;

namespace PDCoreNew
{
    public static class Extensions
    {
        public static string GetErrors(this DbEntityValidationException e)
        {
            return string.Join(Environment.NewLine, e.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
        }

        public static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

            return objectContext.GetTableName<T>();
        }

        public static string GetTableName<T>(this IEntityFrameworkDbContext context) where T : class
        {
            return (context as DbContext).GetTableName<T>();
        }

        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex("FROM (?<table>.*) AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }

        public static string GetQuery<T>(this IEntityFrameworkDbContext context, string where) where T : class
        {
            string tableName = context.GetTableName<T>();

            string query = SqlUtils.SQLQuery(tableName, selection: where);

            return query;
        }

        public static IDisposableWrapper<IEFRepo<TModel>> WrapRepo<TModel>(this IEFRepo<TModel> repo) where TModel : class
        {
            return new SaveChangesWrapper<TModel>(repo);
        }
    }
}
