using Org.BouncyCastle.Utilities.IO;
using PDCore.Extensions;
using PDCore.Interfaces;
using PDCoreNew.Extensions;
using PDCoreNew.Utils;
using System;
using System.Threading.Tasks;

namespace PDCoreNew.Helpers.DataLoaders
{
    public class WebLoader : IDataLoader
    {
        private readonly string _url;

        public WebLoader(string url)
        {
            _url = url;
        }

        public byte[] LoadBytes() => WebUtils.GetResultWithRetryWeb(WebUtils.DownloadData, _url);

        public Task<byte[]> LoadBytesAsync() => WebUtils.GetResultWithRetryWeb(WebUtils.DownloadDataAsync, _url);

        public string LoadString() => WebUtils.GetResultWithRetryWeb(WebUtils.GetTextFromWebClient, _url);

        public Task<string> LoadStringAsync() => WebUtils.GetResultWithRetryWeb(WebUtils.DownloadStringAsync, _url);
    }
}
