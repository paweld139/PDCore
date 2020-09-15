using System;
using System.Globalization;
using System.Web.Mvc;

namespace PDWebCore.Helpers.ModelBinding.MVC
{
    public class DateTimeFormatModelBinder : DefaultModelBinder
    {
        private readonly string _customFormat;

        public DateTimeFormatModelBinder(string customFormat)
        {
            _customFormat = customFormat;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            _ = controllerContext;

            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            return DateTime.ParseExact(value.AttemptedValue, _customFormat, CultureInfo.InvariantCulture);
        }
    }
}
