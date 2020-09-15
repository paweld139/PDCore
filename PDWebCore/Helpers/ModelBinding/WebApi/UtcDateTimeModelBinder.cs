using PDCore.Utils;
using System;
using System.Globalization;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace PDWebCore.Helpers.ModelBinding.WebApi
{
    public class UtcDateTimeModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var stringValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName)?.AttemptedValue;

            if (!DateTime.TryParse(stringValue, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AdjustToUniversal, out DateTime parsedDate))
            {
                return false;
            }

            if (parsedDate.Kind != DateTimeKind.Utc)
            {
                string timezoneStr = Utils.GetHeaderValue(actionContext, Utils.TimezoneOffsetHeaderName);

                if (string.IsNullOrEmpty(timezoneStr))
                {
                    timezoneStr = Utils.GetCookieValue(actionContext, Utils.TimezoneOffsetCookieName);
                }

                if (!string.IsNullOrEmpty(timezoneStr) && int.TryParse(timezoneStr, out int timezoneOffset))
                {
                    parsedDate = DateTimeUtils.DeleteOffset(parsedDate, timezoneOffset);
                }
            }

            bindingContext.Model = parsedDate;

            return true;
        }
    }
}
