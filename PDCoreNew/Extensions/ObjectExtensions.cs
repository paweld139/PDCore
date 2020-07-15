﻿using PDCore.Extensions;
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
using System.Text.RegularExpressions;
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
    }
}