using PDCore.Interfaces;
using PDCore.MVVM;
using PDCore.Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PDWebCore.MVVM
{
    public abstract class ViewModelBaseWeb<T> : ViewModelBase<T> where T : class, IEntity, new()
    {
        private readonly ModelStateDictionary modelState;

        protected ViewModelBaseWeb(IRepository<T> repository, ModelStateDictionary modelState) : base(repository)
        {
            this.modelState = modelState;
        }

        public override void HandleRequest()
        {
            IsValid = modelState.IsValid;

            base.HandleRequest();

            if (IsValid)
            {  // NOTE: Must clear the model state in order to bind
               //       the @Html helpers to the new model values
                modelState.Clear();
            }
            else
            {
                foreach (KeyValuePair<string, string> item in ValidationErrors)
                {
                    modelState.AddModelError(item.Key, item.Value);
                }
            }
        }
    }
}
