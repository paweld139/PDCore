using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
}
