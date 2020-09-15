using Newtonsoft.Json;
using PDCoreNew.Services.Serv;
using System;
using System.Web.Mvc;

namespace PDWebCore.Attributes.MVC
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
                catch(Exception ex)
                {
                    LogService.Error("Błąd podczas ładowaniu modelu z JSON", ex);

                    return null;
                }
            }
        }
    }
}
