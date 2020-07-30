using PDCore.Exceptions;
using PDCore.Extensions;
using PDCore.Interfaces;
using PDCore.Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Repositories.Repo
{
    public class InMemoryRepository<T> : IRepository<T> where T : IEntity
    {
        private List<T> entities = new List<T>();


        private int GetNextId() => entities.Max(e => e.Id) + 1;

        private void SetEntityId(T newEntity)
        {
            newEntity.Id = GetNextId();
        }

        private void SetEntitiesId(IEnumerable<T> newEntities)
        {
            int nextId = GetNextId();

            newEntities.ForEach(e => e.Id = nextId++);
        }


        public void Add(T newEntity)
        {
            if (newEntity.IsValid())
            {
                SetEntityId(newEntity);

                entities.Add(newEntity);
            }
        }

        public void AddRange(IEnumerable<T> newEntities)
        {
            newEntities = newEntities.Where(e => e.IsValid());

            SetEntitiesId(newEntities);

            entities.AddRange(newEntities);
        }


        public void Delete(T entity)
        {
            entities.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            entities.ForEach(e => Delete(e));
        }


        public IQueryable<T> FindAll()
        {
            throw new NotSupportedFunctionalityException();
        }

        public T FindById(int id)
        {
            return entities.SingleOrDefault(e => e.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return entities;
        }

        public int GetCount()
        {
            return entities.Count;
        }


        public int Commit()
        {
            throw new NotSupportedFunctionalityException();
        }


        public void Dispose()
        {
            entities = null;
        }
    }
}
