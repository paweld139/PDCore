using PDWebCore.Context.IContext;
using PDWebCore.Services.Serv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Threading.Tasks;
using PDWebCore.Helpers.ExceptionHandling;

namespace PDWebCore.Filters
{
    public class LogExceptionFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            HttpApplicationErrorHandler.HandleException(context.Exception, LogType.Error);
        }
    }
}
