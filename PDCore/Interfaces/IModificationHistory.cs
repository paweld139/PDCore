using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PDCore.Interfaces
{
    public interface IModificationHistory : IHasRowVersion
    {
        DateTime DateModified { get; set; }

        DateTime DateCreated { get; set; }

        bool IsDirty { get; set; }
    }

    public interface IHasRowVersion
    {
        [Timestamp]
        byte[] RowVersion { get; set; }
    }
}
