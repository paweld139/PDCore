using PDCore.Interfaces;
using System;

namespace PDCore.Models
{
    public class EntityInfo<TKey> where TKey : IEquatable<TKey>
    {
        public EntityInfo(IEntity<TKey> entity)
        {
            Id = entity.Id;
        }

        public TKey Id { get; set; }
    }
}
