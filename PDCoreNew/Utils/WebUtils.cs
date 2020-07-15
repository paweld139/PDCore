using PDCore.Extensions;
using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Utils
{
    public static class WebUtils
    {
        private static Task<string> DoGetTextFromWebClient(string address, WebClient webClient, bool sync)
        {
            ObjectUtils.ThrowIfNull(address.GetTuple(nameof(address)), webClient.GetTuple(nameof(webClient)));

            if (sync)
                return Task.FromResult(webClient.DownloadString(address));

            return webClient.DownloadStringTaskAsync(address);
        }

        private static async Task<string> DoGetTextFromWebClient(string address, bool sync)
        {
            using (WebClient webClient = new WebClient())
            {
                Task<string> task = DoGetTextFromWebClient(address, webClient, sync);

                if (sync)
                    return task.Result;

                return await task;
            }
        }

        public static string GetTextFromWebClient(string address)
        {
            return DoGetTextFromWebClient(address, true).Result;
        }

        public static Task<string> GetTextAsyncFromWebClient(string address)
        {
            return DoGetTextFromWebClient(address, false);
        }

        public static string GetFileNameAfterRead(WebClient webClient)
        {
            string fileName = null;

            string headerContentDisposition = webClient.ResponseHeaders["Content-Disposition"];

            if (!string.IsNullOrEmpty(headerContentDisposition))
                fileName = new ContentDisposition(headerContentDisposition).FileName;

            return fileName;
        }

        private static async Task<string> DoSaveFileFromWebClient(string address, WebClient webClient, bool sync)
        {
            byte[] data;

            if (sync)
                data = webClient.DownloadData(address);
            else
                data = await webClient.DownloadDataTaskAsync(address);

            string fileName = GetFileNameAfterRead(webClient);

            string saveFileLocation = SecurityUtils.GetTempFilePath(fileName);

            if (sync)
                File.WriteAllBytes(saveFileLocation, data);
            else
                await IOUtils.WriteAllBytesAsync(saveFileLocation, data);

            return saveFileLocation;
        }

        private static async Task<string> DoSaveFileFromWebClient(string address, bool sync)
        {
            using (WebClient webClient = new WebClient())
            {
                Task<string> task = DoSaveFileFromWebClient(address, webClient, sync);

                if (sync)
                    return task.Result;

                return await task;
            }
        }

        public static string SaveFileFromWebClient(string address, WebClient webClient)
        {
            return DoSaveFileFromWebClient(address, webClient, true).Result;
        }

        public static Task<string> SaveFileAsyncFromWebClient(string address, WebClient webClient)
        {
            return DoSaveFileFromWebClient(address, webClient, false);
        }

        public static string SaveFileFromWebClient(string address)
        {
            return DoSaveFileFromWebClient(address, true).Result;
        }

        public static Task<string> SaveFileAsyncFromWebClient(string address)
        {
            return DoSaveFileFromWebClient(address, false);
        }

        public static async Task<string[]> DownloadStringsAsync(string[] urls)
        {
            var tasks = new Task<string>[urls.Length];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = DownloadStringAsync(urls[i]);
            }

            return await Task.WhenAll(tasks);
        }

        public static async Task<string> DownloadStringAsync(string url)
        {
            //validate!
            using (var client = new WebClient())
            {
                //optionally process and return
                return await client.DownloadStringTaskAsync(url).ConfigureAwait(false);
            }
        }
    }
}
