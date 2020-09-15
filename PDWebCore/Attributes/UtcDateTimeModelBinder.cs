using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace PDWebCore.Attributes
{
    public class UtcDateTimeAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new UtcDateTimeModelBinder();
        }

        public class UtcDateTimeModelBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                if (bindingContext == null)
                {
                    throw new ArgumentNullException(nameof(bindingContext));
                }

                if (bindingContext.ModelMetadata.ModelType == typeof(DateTime))
                {
                    var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                    var str = valueProviderResult.AttemptedValue;

                    return DateTime.Parse(str).ToUniversalTime();
                }

                return null;
            }
        }
    }

    public class UtcDateTimeModelBinder : System.Web.Http.ModelBinding.IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, System.Web.Http.ModelBinding.ModelBindingContext bindingContext)
        {
            var stringValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName)?.RawValue as string;

            if (!DateTime.TryParse(stringValue, null, DateTimeStyles.AdjustToUniversal, out DateTime parsedDate))
            {
                return false;
            }

            bindingContext.Model = parsedDate;

            return true;
        }
    }
}
