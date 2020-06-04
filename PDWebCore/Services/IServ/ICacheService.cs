using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDWebCore.Services.IServ
{
    public interface ICacheService
    {
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class;
    }
}
