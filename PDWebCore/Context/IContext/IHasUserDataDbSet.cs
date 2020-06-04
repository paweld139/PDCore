using PDWebCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDWebCore.Context.IContext
{
    public interface IHasUserDataDbSet
    {
        DbSet<UserDataModel> UserData { get; set; }
    }
}
