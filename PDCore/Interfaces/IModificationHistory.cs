using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Interfaces
{
    public interface IModificationHistory
    {
        DateTime DateModified { get; set; }
        DateTime DateCreated { get; set; }
        bool IsDirty { get; set; }
    }
}
