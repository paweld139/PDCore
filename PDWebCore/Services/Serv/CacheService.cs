using PDCore.Services;
using PDCoreNew.Services.IServ;
using PDWebCore.Services.IServ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace PDWebCore.Services.Serv
{
    public class CacheService : ICacheService
    {
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            if (!(HttpContext.Current.Cache.Get(cacheKey) is T item))
            {
                item = getItemCallback();

                HttpContext.Current.Cache.Insert(cacheKey, item, null, DateTime.UtcNow.AddDays(1), Cache.NoSlidingExpiration);
            }

            return item;
        }
    }
}
