using PDCore.Extensions;
using PDCore.Helpers.Wrappers.DisposableWrapper;
using PDCore.Utils;
using PDCoreNew.Context.IContext;
using PDCoreNew.Helpers;
using PDCoreNew.Repositories.IRepo;
using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity;

namespace PDCoreNew.Extensions
{
    public static class ObjectExtensions
    {
        public static string GetErrors(this DbEntityValidationException e)
        {
            return string.Join(Environment.NewLine, e.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
        }

        public static IDisposableWrapper<IEFRepo<TModel>> WrapRepo<TModel>(this IEFRepo<TModel> repo) where TModel : class
        {
            return new SaveChangesWrapper<TModel>(repo);
        }

        public static void RemoveRegistrations(this IUnityContainer container, string name, Type registeredType, Type lifetimeManager)
        {
            foreach (var registration in container.Registrations
                .Where(p => p.RegisteredType == (registeredType ?? p.RegisteredType)
                            && p.Name == (name ?? p.Name)
                            && p.LifetimeManager.GetType() == (lifetimeManager ?? p.LifetimeManager.GetType())))
            {
                registration.LifetimeManager.RemoveValue();
            }
        }

        public static void RemoveRegistrations<TReg, TLife>(this IUnityContainer container, string name = null)
        {
            container.RemoveRegistrations(name, typeof(TReg), typeof(TLife));
        }

        public static void RemoveAllRegistrations(this IUnityContainer container)
        {
            container.RemoveRegistrations(null, null, null);
        }

        private async static Task<TResult> DoWithRetry<TResult, TException>(Func<TResult> func, Func<Task<TResult>> task, bool sync) where TException : Exception
        {
            var result = default(TResult);

            int retryCount = 0;

            bool succesful = false;

            do
            {
                try
                {
                    if (sync)
                        result = func();
                    else
                        result = await task();

                    succesful = true;
                }
                catch (TException)
                {
                    retryCount++;
                }
            } while (retryCount < 3 && !succesful);

            return result;
        }

        public static TResult WithRetry<TResult, TException>(this Func<TResult> func) where TException : Exception
        {
            return DoWithRetry<TResult, TException>(func, null, true).Result;
        }

        public static Task<TResult> WithRetry<TResult, TException>(this Func<Task<TResult>> task) where TException : Exception
        {
            return DoWithRetry<TResult, TException>(null, task, false);
        }

        public static T WithRetryWeb<T>(this Func<T> func)
        {
            return func.WithRetry<T, WebException>();
        }

        public static Task<T> WithRetryWeb<T>(this Func<Task<T>> task)
        {
            return task.WithRetry<T, WebException>();
        }

        public static TResult WithRetry<TResult>(this Func<TResult> func)
        {
            return func.WithRetry<TResult, Exception>();
        }

        public static Task<TResult> WithRetry<TResult>(this Func<Task<TResult>> task)
        {
            return task.WithRetry<TResult, Exception>();
        }
    }
}
