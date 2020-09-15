using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace PDWebCore.Helpers.ModelBinding.WebApi.Providers
{
    public class DateTimeCultureModelBinderProvider : ModelBinderProvider
    {
        readonly DateTimeCultureModelBinder binder = new DateTimeCultureModelBinder();

        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            if (DateTimeCultureModelBinder.CanBindType(modelType))
            {
                return binder;
            }

            return null;
        }
    }
}
