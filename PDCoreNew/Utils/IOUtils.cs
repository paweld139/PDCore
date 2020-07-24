using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;

namespace PDCoreNew.Utils
{
    public static class IOUtils
    {
        public static async Task WriteAllBytesAsync(string path, byte[] data)
        {
            using (FileStream SourceStream = File.Open(path, FileMode.OpenOrCreate))
            {
                SourceStream.Seek(0, SeekOrigin.End);

                await SourceStream.WriteAsync(data, 0, data.Length);
            }
        }

        public static async Task<byte[]> ReadAllBytesAsync(string path)
        {
            byte[] result;

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                result = new byte[stream.Length];

                await stream.ReadAsync(result, 0, (int)stream.Length);
            }

            return result;
        }
    }
}
