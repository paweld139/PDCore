using PDCoreNew.Configuration.DbConfiguration.DataReaders;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Configuration.DbConfiguration.Interceptors
{
    public class UtcInterceptor : DbCommandInterceptor
    {
        public override void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            base.ReaderExecuted(command, interceptionContext);

            if (interceptionContext?.Result != null && !(interceptionContext.Result is UtcDbDataReader))
            {
                interceptionContext.Result = new UtcDbDataReader(interceptionContext.Result);
            }
        }
    }
}
