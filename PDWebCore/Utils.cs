using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Optimization;

namespace PDWebCore
{
    public static class Utils
    {
        public const string TimezoneOffsetHeaderName = "x-timezone-offset";
        public const string TimezoneOffsetCookieName = "timezoneOffset";

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

        public static SelectList GetSelectList(IList<KeyValuePair<int, string>> kvp, bool withCaption = true)
        {
            if (withCaption)
            {
                kvp.Insert(0, new KeyValuePair<int, string>(-1, Resources.Common.Unselected));
            }

            SelectList res = new SelectList(kvp, "Key", "Value");

            return res;
        }

        public static string GetMimeType(string fileName)
        {
            string mimeType = MimeMapping.GetMimeMapping(fileName);

            return mimeType;
        }

        public static void ToggleWebEncrypt(string sectionName = "connectionStrings")
        {
            // Open the Web.config file.
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");

            // Get the connectionStrings section.
            ConfigurationSection section = config.GetSection(sectionName);

            // Toggle encryption.
            if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
            }
            else
            {
                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
            }

            // Save changes to the Web.config file.
            config.Save();
        }

        /// <summary>
        /// Render scripts as deferred
        /// </summary>
        /// <param name="paths">
        /// The paths.
        /// </param>
        /// <returns>
        /// The <see cref="IHtmlString"/>.
        /// </returns>
        public static IHtmlString RenderDefer(params string[] paths)
        {
            return Scripts.RenderFormat(@"<script src='{0}' defer></script>", paths);
        }

        public static string GetHeaderValue(HttpActionContext actionContext, string headerName)
        {
            string result = null;

            var headers = actionContext.Request.Headers;

            if (headers.Contains(headerName))
            {
                result = headers.GetValues(headerName)?.FirstOrDefault();
            }

            return result;
        }

        public static string GetCookieValue(HttpActionContext actionContext, string cookieName)
        {
            string result = null;

            var cookie = actionContext.Request.Headers.GetCookies(cookieName).FirstOrDefault();

            if (cookie != null)
            {
                result = cookie[cookieName].Value;
            }

            return result;
        }

        public static SelectListItem[] GetSelectList()
        {
            var tzs = TimeZoneInfo.GetSystemTimeZones();

            return tzs.Select(tz => new SelectListItem()
            {
                Text = tz.DisplayName,
                Value = tz.Id
            }).ToArray();
        }

        public static string GetUsersTimezone(string defaultTimezone)
        {
            // try to pick up user's timezone
            var jsTime = HttpContext.Current.Request.QueryString["JsTime"];

            string result;

            if (!string.IsNullOrEmpty(jsTime))
                //Wed Feb 04 2015 18:37:55 GMT-1000 (Hawaiian Standard Time) 
                result = StringUtils.ExtractString(jsTime, "(", ")");
            else
                result = defaultTimezone; //App.DefaultTimeZone;

            return result;
        }
    }
}
