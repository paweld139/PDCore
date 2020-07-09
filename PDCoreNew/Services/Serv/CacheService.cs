using PDCoreNew.Services.IServ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Services.Serv
{
    public class CacheService : ICacheService
    {
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            if (!(MemoryCache.Default.Get(cacheKey) is T item))
            {
                item = getItemCallback();

                MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddMinutes(10));
            }

            return item;
        }
    }
}
