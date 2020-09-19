using Microsoft.Ajax.Utilities;
using PDWebCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Validation attribute that demands that a boolean value must be true.
    /// </summary>
    public class EnforceTrueAttribute : ValidationAttribute, IClientValidatable
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            if (value.GetType() != typeof(bool))
                throw new InvalidOperationException("can only be used on boolean properties.");

            return (bool)value == true;
        }

        public override string FormatErrorMessage(string name)
        {
            return "The " + name + " field must be checked in order to continue.";
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? FormatErrorMessage(metadata.DisplayName) : ErrorMessage,
                ValidationType = "enforcetrue"
            };
        }
    }
}

namespace PDWebCore
{
    public static class Extensions
    {
        public static JsonResult JsonGet(this Controller c, object data = null, string message = null, PartialViewResult view = null, bool isError = false, bool isConfirm = false)
        {
            var res = Json(c, data, message, view, isError, isConfirm);

            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return res;
        }

        private static JsonResult Json(Controller c, object data, string message, PartialViewResult view = null, bool isError = false, bool isConfirm = false)
        {
            JsonResult res = new JsonResult();

            var result = new JsonResultModel(data, message, null, isError, isConfirm);


            if (view != null)
            {
                result.View = ConvertPartialViewToString(c, view);
            }

            res.Data = result;

            return res;
        }

        private static string ConvertPartialViewToString(Controller c, PartialViewResult partialView)
        {
            using (var sw = new StringWriter())
            {
                partialView.View = ViewEngines.Engines.FindPartialView(c.ControllerContext, partialView.ViewName).View;

                var vc = new ViewContext(c.ControllerContext, partialView.View, partialView.ViewData, partialView.TempData, sw);

                partialView.View.Render(vc, sw);

                var partialViewString = sw.GetStringBuilder().ToString();

                return partialViewString;
            }
        }

        public static JsonResult JsonPost(this Controller c, object data = null, string message = null, PartialViewResult view = null, bool isError = false, bool isConfirm = false)
        {
            return Json(c, data, message, view, isError, isConfirm);
        }

        public static string GetModelStateErrors(this Controller c)
        {
            string result = string.Join(Environment.NewLine, c.ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));

            return result;
        }

        public static bool IsAjaxRequest(this HttpRequest httpResponse)
        {
            return (httpResponse.Headers["X-Requested-With"] == "XMLHttpRequest");
        }

        public static string UrlEncodeUppercaseUTF8(this string content)
        {
            string lower = HttpUtility.UrlEncode(content, Encoding.UTF8);

            Regex reg = new Regex(@"%[a-f0-9]{2}");

            string upper = reg.Replace(lower, m => m.Value.ToUpperInvariant());

            return upper;
        }

        private const string LogId = "LOG_ID";

        public static void SetLogId(this HttpRequestMessage request, Guid id)
        {
            request.Properties[LogId] = id;
        }

        public static Guid GetLogId(this HttpRequestMessage request)
        {
            if (request.Properties.TryGetValue(LogId, out object value))
            {
                return (Guid)value;
            }

            return Guid.Empty;
        }

        public static MvcHtmlString HashLink(this HtmlHelper htmlHelper, string text, string className = "")
        {
            _ = htmlHelper;

            var anchor = new TagBuilder("a")
            {
                InnerHtml = text
            };

            anchor.Attributes.Add("href", "#");

            if (!string.IsNullOrWhiteSpace(className))
            {
                anchor.AddCssClass(className);
            }

            return MvcHtmlString.Create(anchor.ToString());
        }

        public static MvcHtmlString ActionLinkWithHash(this HtmlHelper htmlHelper, string linkText, string hashText, string actionName, string controllerName)
        {
            _ = htmlHelper;

            var requestContext = HttpContext.Current.Request.RequestContext;

            var urlHelper = new UrlHelper(requestContext);

            var urlAction = urlHelper.Action(actionName, controllerName);

            var anchor = new TagBuilder("a")
            {
                InnerHtml = linkText
            };

            anchor.Attributes.Add("href", $"{urlAction}#{hashText}");

            return MvcHtmlString.Create(anchor.ToString());
        }

        public static StatusCodeResult Forbid(this ApiController controller) => new StatusCodeResult(HttpStatusCode.Forbidden, controller);

        public static StatusCodeResult NoContent(this ApiController controller) => new StatusCodeResult(HttpStatusCode.NoContent, controller);

        public static string ToClientTime(this DateTime dt)
        {
            // read the value from session
            var timeOffSet = HttpContext.Current.Session[Utils.TimezoneOffsetCookieName];

            if (timeOffSet != null)
            {
                var offset = int.Parse(timeOffSet.ToString());
                dt = dt.AddMinutes(-1 * offset);

                return dt.ToString();
            }

            // if there is no offset in session return the datetime in server timezone
            return dt.ToLocalTime().ToString();
        }
    }
}
