using PDWebCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDWebCore.Context.IContext
{
    public interface IHasErrorLogDbSet
    {
        DbSet<LogModel> ErrorLog { get; set; }
    }
}
