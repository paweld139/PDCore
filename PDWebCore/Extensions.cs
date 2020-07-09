﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.IO;
using PDWebCore.Models;
using System.Web;
using System.Text.RegularExpressions;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using PDCore.Context.IContext;
using PDWebCore.Context.IContext;
using System.Data.Entity.Validation;
using PDCore.Helpers;
using PDWebCore.Helpers.ExceptionHandling;
using PDWebCore.Helpers;
using System.Net.Http;
using PDCore.Helpers.Wrappers.DisposableWrapper;
using PDCore.Utils;
using PDCoreNew.Context.IContext;
using PDCoreNew.Repositories.IRepo;
using PDCoreNew.Helpers;

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
            if (value.GetType() != typeof(bool)) throw new InvalidOperationException("can only be used on boolean properties.");
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
    }
}
