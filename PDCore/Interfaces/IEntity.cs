using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Interfaces
{
    public interface IEntity : IEntity<int>
    {
        bool IsValid();

        List<KeyValuePair<string, string>> ValidationErrors { get; }
    }

    public interface IEntity<TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }
}
