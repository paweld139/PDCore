using PDCore.Enums;
using PDCoreNew.Factories.IFac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Factories.Fac
{
    public class LogMessageFactory : ILogMessageFactory
    {
        public string Create(string message, Exception exception, LogType logType)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("{0}: ", logType);

            var items = new[] { message, exception?.ToString() };

            string itemsText = string.Join(", Wyjątek: ", items.Where(x => !string.IsNullOrEmpty(x)));

            result.Append(itemsText);


            return result.ToString();
        }
    }
}
