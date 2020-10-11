using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PDWebCore.Filters.WebApi
{
    public class ModelValidateNotNullFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments.Any(v => v.Value == null))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
