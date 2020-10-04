using PDCore.Interfaces;
using PDCoreNew.Utils;
using System.IO;
using System.Threading.Tasks;

namespace PDCoreNew.Helpers.DataLoaders
{
    public class LocalLoader : IDataLoader
    {
        private readonly string _fileName;

        public LocalLoader(string fileName)
        {
            _fileName = fileName;
        }

        public byte[] LoadBytes()
        {
            return File.ReadAllBytes(_fileName);
        }

        public Task<byte[]> LoadBytesAsync()
        {
            return IOUtils.ReadAllBytesAsync(_fileName);
        }

        public string LoadString()
        {
            return File.ReadAllText(_fileName);
        }

        public Task<string> LoadStringAsync()
        {
            return IOUtils.ReadAllTextAsync(_fileName);
        }
    }
}
