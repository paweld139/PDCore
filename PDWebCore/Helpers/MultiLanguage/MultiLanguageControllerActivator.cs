using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace PDWebCore.Helpers.MultiLanguage
{
    public class MultiLanguageControllerActivator : IControllerActivator 
    { 
        public IController Create(RequestContext requestContext, Type controllerType) 
        { 
            //LanguageHelper.SetLanguage(requestContext.HttpContext.Request); 

            return DependencyResolver.Current.GetService(controllerType) as IController; 
        } 
    }
}
