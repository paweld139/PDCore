using PDCoreNew.Services.Serv;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PDWebCore.Helpers.MultiLanguage
{
    public static class LanguageHelper
    {
        public static readonly List<Language> AvailableLanguages = new List<Language>
        {
            new Language("English", "en"),
            new Language("Polski", "pl"),
        };

        public static bool IsLanguageAvailable(string lang) => AvailableLanguages.Exists(a => a.LanguageCultureName.Equals(lang));

        public static string GetDefaultLanguage() => AvailableLanguages[0].LanguageCultureName;

        private static string GetLanguage(HttpRequest httpRequest)
        {
            HttpCookie languageCookie = httpRequest.Cookies["culture"];

            string language;

            if (languageCookie != null)
            {
                language = languageCookie.Value;
            }
            else
            {
                var userLanguages = httpRequest.UserLanguages;
                var userLanguage = userLanguages != null ? userLanguages[0] : string.Empty;

                if (!string.IsNullOrEmpty(userLanguage))
                {
                    language = userLanguage;
                }
                else
                {
                    language = GetDefaultLanguage();
                }
            }

            return language;
        }

        public static void SetLanguage(string language)
        {
            try
            {
                var cultureInfo = new CultureInfo(language);

                if (!IsLanguageAvailable(cultureInfo.TwoLetterISOLanguageName))
                    language = GetDefaultLanguage();

                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(language);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);

                HttpCookie langCookie = new HttpCookie("culture", language)
                {
                    Expires = DateTime.UtcNow.AddYears(1)
                };

                HttpContext.Current.Response.Cookies.Add(langCookie);
            }
            catch(Exception ex)
            {
                LogService.Error("Błąd podczas ustawiania języka", ex);
            }
        }

        public static void SetLanguage(HttpRequest httpRequest)
        {
            string language = GetLanguage(httpRequest);

            SetLanguage(language);
        }
    }
}
