using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;

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
    }
}
