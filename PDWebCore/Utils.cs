using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace PDWebCore
{
    public static class Utils
    {
        public static string GetIPAddress()
        {
            HttpContext context = HttpContext.Current;

            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');

                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public static SelectList GetSelectList(List<KeyValuePair<int, string>> kvp, bool withCaption = true)
        {
            if (withCaption)
            {
                kvp.Insert(0, new KeyValuePair<int, string>(-1, "Wybierz..."));
            }

            SelectList res = new SelectList(kvp, "Key", "Value");

            return res;
        }

        public static string GetMimeType(string fileName)
        {
            string mimeType = MimeMapping.GetMimeMapping(fileName);

            return mimeType;
        }
    }
}
