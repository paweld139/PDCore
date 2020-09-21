using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PDWebCore.Filters.WebApi
{
    public class ValidationActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            //var modelState = context.ModelState;

            //if (!modelState.IsValid)
            //{
            //    //InvalidModelStateResult result = new InvalidModelStateResult(modelState, (ApiController)context.ControllerContext.Controller);

            //    //context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, result);

            //    var errors = new JObject();

            //    foreach (var key in modelState.Keys)
            //    {
            //        var state = modelState[key];

            //        if (state.Errors.Any())
            //        {
            //            errors[key] = state.Errors.First().ErrorMessage;
            //        }
            //    }

            //    context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            //}

            var modelState = context.ModelState;

            if (!modelState.IsValid)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
            }

            //base.OnActionExecuting(context);
        }
    }
}
