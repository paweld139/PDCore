using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Interfaces
{
    public interface IDTO<TKey>
    {
        TKey Id { get; set; }
    }

    public interface IDTO : IDTO<int>
    {

    }
}
