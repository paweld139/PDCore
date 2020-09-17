using PDCore.Helpers.Wrappers.DisposableWrapper;
using PDCore.Interfaces;
using PDCore.Repositories.IRepo;

namespace PDCoreNew.Helpers
{
    public class SaveChangesWrapper<TModel> : DisposableWrapper<ISqlRepositoryEntityFramework<TModel>> where TModel : class, IModificationHistory
    {
        private readonly bool withoutValidation;

        public SaveChangesWrapper(ISqlRepositoryEntityFramework<TModel> repo, bool withoutValidation) : base(repo) 
        {
            this.withoutValidation = withoutValidation;
        }

        protected override void OnDispose()
        {
            if (!withoutValidation)
            {
                // lots of code per state of BaseObject
                BaseObject.CommitWithoutValidation();
            }
            else
            {
                BaseObject.Commit();
            }
        }
    }
}
