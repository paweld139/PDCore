using PDCoreNew.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Context.IContext
{
    public interface IHasLogDbSet
    {
        DbSet<LogModel> ErrorLog { get; set; }
    }
}
