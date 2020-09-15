using PDCore.Enums;
using PDWebCore.Helpers.ExceptionHandling;
using System.Web.Mvc;

namespace PDWebCore.Filters.MVC
{
    public class LogExceptionFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            HttpApplicationErrorHandler.HandleException(context.Exception, LogType.Error);
        }
    }
}
