using PDCoreNew.Models;
using System.Collections.Generic;

namespace PDCoreNew.Interfaces
{
    public interface IHasFiles
    {
        ICollection<FileModel> Files { get; set; }
    }
}
