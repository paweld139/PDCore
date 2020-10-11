using PDCore.Helpers;
using PDCore.Interfaces;
using PDCore.Models;
using PDCore.Repositories.IRepo;
using System;
using System.Linq.Expressions;

namespace PDCore.Utils
{
    public static class RepositoryUtils
    {
        public const string DeleteDataExceptionMessage = "Unable to delete. Try again, and if the problem persists contact your system administrator.";

        public const string ConcurrencyDeleteErrorMessage = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";

        public static void DumpItems<T>(IReadOnlyRepository<T> repository, Action<T> print = null) where T : class
        {
            if (print == null)
                print = Console.WriteLine;

            var result = repository.FindAll();

            foreach (var item in result)
            {
                print(item);
            }
        }

        //Out - bardziej szczegółowe typy zostaną zwrócone jako mniej szczegółowe
        public static void DumpNamedObject(IReadOnlyRepository<NamedObject> repository, Action<NamedObject> print = null)
        {
            if (print == null)
                print = i => Console.WriteLine(i.Name);

            DumpItems(repository, print);
        }

        public static void SetLogging(bool input, ILogger logger, bool isLoggingEnabled, Action enableLogging, Action disableLogging)
        {
            if (input == isLoggingEnabled || logger == null)
            {
                return;
            }

            if (input)
                enableLogging();
            else
                disableLogging();
        }

        /// <summary>
        /// Produces a predicate for IQueryable LINQ "Where" clause that queries by id for specific value,
        /// </summary>
        /// <typeparam name="T">The type to query which must have an int property named "Id".</typeparam>
        /// <param name="id">The int value of the id of the desired entity.</param>
        /// <returns>An predicate expression suitable for a LINQ "Where" or "First" clause.</returns>
        /// <remarks>
        /// See <see cref="Model.IRepository{T}.GetById"/> for usage.
        /// </remarks>
        /// <Example>
        /// If T is a Foo and Foo.Id is of type int, then
        /// var predicate = GetByIdPredicate{T}(42) returns the equivalent of 
        /// "f => f.Id == 42" and can be used to get the Foo with Id==42 by writing
        /// aFooDbSet.FirstOrDefault(predicate)".
        /// </Example>
        public static Expression<Func<T, bool>> GetByIdPredicate<T>(long id)
        {
            var itemParam = Expression.Parameter(typeof(T), "item"); //Parametr wyrażenia
            var itemPropertyExpr = Expression.Property(itemParam, "Id"); //Wyrażenie pozwalające na wyciągnięcie właściwości
            var idParam = Expression.Constant(id); //Wartość
            var newBody = Expression.MakeBinary(ExpressionType.Equal, itemPropertyExpr, idParam); //Wyrażenie przyrównujące właściwość parametru z wartością
            var newLambda = Expression.Lambda(newBody, itemParam); //Utworzenie wyrażenia lambda na podstawie ciała wyrażenia i parametru

            return newLambda as Expression<Func<T, bool>>;
        }
    }
}
