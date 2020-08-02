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


        private int GetNextId() => entities.Select(e => e.Id).DefaultIfEmpty(0).Max() + 1;

        private void SetEntityId(T newEntity)
        {
            newEntity.Id = GetNextId();
        }

        private void SetEntitiesId(IEnumerable<T> newEntities)
        {
            int nextId = GetNextId();

            foreach (var item in newEntities)
            {
                item.Id = nextId++;
            }
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

        public void Update(T entity)
        {
            int index = entities.FindIndex(e => e.Id == entity.Id);

            entities[index] = entity;
        }


        public void Delete(T entity)
        {
            entities.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            foreach (var item in entities)
            {
                Delete(item);
            }
        }


        public IQueryable<T> FindAll()
        {
            return GetAll().AsQueryable();
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
            throw new NotSupportedException();
        }


        public void Dispose()
        {
            entities = null;
        }
    }
}
