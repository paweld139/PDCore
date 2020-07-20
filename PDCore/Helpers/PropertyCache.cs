using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PDCore.Helpers
{
    public static class PropertyCache<T>
    {
        private static readonly Lazy<IEnumerable<PropertyInfo>> publicPropertiesLazy = new Lazy<IEnumerable<PropertyInfo>>(
            () => Array.AsReadOnly(typeof(T).GetProperties()));

        public static IEnumerable<PropertyInfo> PublicProperties => publicPropertiesLazy.Value;
    }
}
