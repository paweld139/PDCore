using Newtonsoft.Json;
using System.Web.Mvc;

namespace PDWebCore.Attributes
{
    public class FromJsonAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new JsonModelBinder();
        }

        public class JsonModelBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                try
                {
                    var json = controllerContext.HttpContext.Request.Form[bindingContext.ModelName];

                    // Swap this out with whichever Json deserializer you prefer.
                    return JsonConvert.DeserializeObject(json, bindingContext.ModelType);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
