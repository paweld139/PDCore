using Microsoft.AspNet.Identity.EntityFramework;
using PDCore.Extensions;
using PDCore.Interfaces;
using PDCore.Utils;
using PDCoreNew.Extensions;
using PDCoreNew.Models;
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
    public abstract class MainWebDbContext<TUser> : IdentityDbContext<TUser>, IMainWebDbContext where TUser : IdentityUser
    {
        public MainWebDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString, throwIfV1Schema: false)
        {
        }

        public DbSet<LogModel> ErrorLog { get; set; }

        //public DbSet<FileModel> File { get; set; }

        public DbSet<UserDataModel> UserData { get; set; }

        public bool IsLoggingEnabled => this.IsLoggingEnabled();

        public void SetLogging(bool input, ILogger logger)
        {
            this.SetLogging(input, logger, IsLoggingEnabled);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureForModificationHistory();

            base.OnModelCreating(modelBuilder);
        }
    }
}
