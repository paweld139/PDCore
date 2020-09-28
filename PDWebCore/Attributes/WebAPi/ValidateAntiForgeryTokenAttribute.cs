using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PDWebCore.Attributes.WebAPi
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            try
            {
                //AntiForgery.Validate();

                var headers = actionContext.Request.Headers;
                var cookie = headers
                    .GetCookies()
                    .Select(c => c[AntiForgeryConfig.CookieName])
                    .FirstOrDefault();
                var rvt = headers.GetValues("__RequestVerificationToken").FirstOrDefault();
                AntiForgery.Validate(cookie?.Value, rvt);
            }
            catch
            {
                actionContext.Response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    RequestMessage = actionContext.ControllerContext.Request
                };

                return FromResult(actionContext.Response);
            }

            return continuation();
        }

        private Task<HttpResponseMessage> FromResult(HttpResponseMessage result)
        {
            var source = new TaskCompletionSource<HttpResponseMessage>();

            source.SetResult(result);

            return source.Task;
        }
    }
}
