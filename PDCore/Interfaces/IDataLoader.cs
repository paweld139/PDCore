using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Interfaces
{
    public interface IDataLoader
    {
        string LoadString();

        byte[] LoadBytes();

        Task<string> LoadStringAsync();

        Task<byte[]> LoadBytesAsync();
    }
}
