using PDCore.Helpers;
using PDCore.Repositories.IRepo;
using PDWebCore.Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDWebCore.Helpers
{
    public class SaveChangesWrapper<TModel> : DisposableWrapper<IEFRepo<TModel>> where TModel : class
    {
        public SaveChangesWrapper(IEFRepo<TModel> repo) : base(repo) { }

        protected override void OnDispose()
        {
            // lots of code per state of BaseObject
            BaseObject.SaveChanges();
        }
    }
}
