using PDCore.Interfaces;
using PDCore.Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.MVVM
{

    public abstract class ViewModelBase<T> where T : class, IEntity, new()
    {
        private readonly IRepository<T> repository;

        public T Entity { get; protected set; }
        public IEnumerable<T> Entities { get; protected set; }
        public T SearchEntity { get; protected set; }

        public string EventCommand { get; set; }
        public string EventArgument { get; set; }
        public string Mode { get; set; }
        public bool IsValid { get; set; }
        public List<KeyValuePair<string, string>> ValidationErrors { get; protected set; }

        public bool IsDetailAreaVisible { get; protected set; }
        public bool IsListAreaVisible { get; protected set; }
        public bool IsSearchAreaVisible { get; protected set; }

        protected ViewModelBase(IRepository<T> repository)
        {
            this.repository = repository;
            Init();
        }

        protected virtual void Init()
        {
            Entity = new T();

            Entities = new List<T>();

            SearchEntity = new T();

            EventCommand = "list";

            ValidationErrors = new List<KeyValuePair<string, string>>();

            ListMode();
        }

        public virtual void HandleRequest()
        {
            switch (EventCommand.ToLower())
            {
                case "list":
                case "search":
                    Get();
                    break;

                case "save":
                    Save();
                    break;

                case "edit":
                    Edit();
                    break;

                case "delete":
                    ResetSearch();
                    Delete();
                    Get();
                    break;

                case "cancel":
                    ListMode();
                    Get();
                    break;

                case "resetsearch":
                    ResetSearch();
                    Get();
                    break;

                case "add":
                    Add();
                    break;

                default:
                    break;
            }
        }

        protected virtual void ListMode()
        {
            IsValid = true;

            IsListAreaVisible = true;
            IsSearchAreaVisible = true;
            IsDetailAreaVisible = false;

            Mode = "List";
        }

        protected abstract Func<T, bool> Search(T searchEntity);

        protected virtual void ResetSearch()
        {
            IsValid = true;

            SearchEntity = new T();
        }

        protected virtual void AddMode()
        {
            IsListAreaVisible = false;
            IsSearchAreaVisible = false;
            IsDetailAreaVisible = true;

            Mode = "Add";
        }

        protected virtual void EditMode()
        {
            IsListAreaVisible = false;
            IsSearchAreaVisible = false;
            IsDetailAreaVisible = true;

            Mode = "Edit";
        }

        protected void Get()
        {
            var predicate = Search(SearchEntity);

            Entities = repository.FindAll().Where(predicate).ToList();
        }

        protected virtual void Delete()
        {
            int id = Convert.ToInt32(EventArgument);

            Entity = repository.FindById(id);

            repository.Delete(Entity);

            ListMode();
        }

        protected abstract void InitializeForAdd(T entity);

        protected virtual void Add()
        {
            IsValid = true;

            InitializeForAdd(Entity);

            AddMode();
        }

        protected virtual void Edit()
        {
            IsValid = true;

            int id = Convert.ToInt32(EventArgument);

            Entity = repository.FindById(id);

            EditMode();
        }

        protected virtual void Save()
        {
            int id = Convert.ToInt32(EventArgument);

            Entity.Id = id;

            IsValid &= Entity.IsValid();

            ValidationErrors = Entity.ValidationErrors ?? new List<KeyValuePair<string, string>>();

            if (IsValid)
            {
                if (Mode == "Add")
                {
                    repository.Add(Entity);
                }
                else
                {
                    repository.Update(Entity);
                }

                Get();
            }
            else
            {
                if (Mode == "Add")
                {
                    AddMode();
                }
                else
                {
                    EditMode();
                }
            }
        }
    }
}
