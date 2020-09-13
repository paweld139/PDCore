using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Models
{
    /// <summary>
    /// A and the ids of every object that has this tag.
    /// </summary>
    /// <remarks>
    /// This is a DTO, not an entity backed by a database object
    /// </remarks>
    public class TagGroup
    {
        public string Tag { get; set; }
        public ICollection<int> Ids { get; set; }
    }
}
