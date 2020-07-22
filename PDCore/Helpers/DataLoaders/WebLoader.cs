using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace PDCore.Helpers.DataLoaders
{
    public class WebLoader : IDataLoader
    {
        private readonly string _url;

        public WebLoader(string url)
        {
            _url = url;
        }

        public string LoadData()
        {
            var client = new WebClient();

            return client.DownloadString(new Uri(_url));
        }
    }
}
