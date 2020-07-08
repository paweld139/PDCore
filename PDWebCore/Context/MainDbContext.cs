using Microsoft.AspNet.Identity.EntityFramework;
using PDCore.Interfaces;
using PDWebCore.Context.IContext;
using PDWebCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PDWebCore.Context
{
    public abstract class MainDbContext<TUser> : IdentityDbContext<TUser>, IMainDbContext where TUser : IdentityUser
    {
        public MainDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString, throwIfV1Schema: false)
        {
        }

        public DbSet<LogModel> ErrorLog { get; set; }

        public DbSet<FileModel> File { get; set; }

        public DbSet<UserDataModel> UserData { get; set; }


        public bool ExistsLocal<T>(Func<T, bool> predicate) where T : class
        {
            return this.Set<T>().Local.Any(predicate);
        }

        public T FirstLocal<T>(Func<T, bool> predicate) where T : class
        {
            return this.Set<T>().Local.First(predicate);
        }

        public bool ExistsLocal<T>(T entity) where T : class
        {
            return this.Set<T>().Local.Any(e => e == entity);
        }


        public bool IsLoggingEnabled { get; private set; }

        public void SetLogging(bool res, ILogger logger)
        {
            if (res == IsLoggingEnabled || logger == null)
            {
                return;
            }

            IsLoggingEnabled = res;

            if (res)
            {
                Database.Log = logger.Log;
            }
            else
            {
                Database.Log = null;
            }
        }
    }
}
