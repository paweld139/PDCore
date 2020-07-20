using PDCore.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace PDCore.Utils
{
    public static class WebUtils
    {
        public const string ResultOkIndicator = "ok";

        public readonly static Dictionary<string, object> Errors = new Dictionary<string, object>
        {
            {"RNF", "Nie odnaleziono zasobu"},
            {"AD", "Nie masz dostępu do tego zasobu"},
            {"ER", "Wystąpił błąd"},
            {"NF", "Brak danych"}
        };

        public static string MakeError(string message)
        {
            return "error_" + message;
        }

        public static string MakeError(Exception exception)
        {
            return MakeError(exception.Message);
        }

        public static string MakeConfirm(string message)
        {
            return "confirm_" + message;
        }

        public static bool WithoutErrors(string result)
        {
            return result == ResultOkIndicator;
        }

        public static string JSScript(string content)
        {
            string result = string.Format("<script>{0}</script>", content);

            return result;
        }

        public static string JSScriptAlert(string text, bool historyBack = false, bool reload = false, string href = "")
        {
            string result = "<script>(function(){ alert('" + text + "');" + (historyBack == true ? "history.back();" : "") + (reload == true ? "location.reload();" : "") + (href != "" ? string.Format("location.href = '{0}';", href) : "") + " })()</script>";

            return result;
        }

        public const string br = "<br />";

        public const string hr = "<hr>";

        public static string ItallicP(string content)
        {
            string result = string.Format("<p style=\"font-style: italic\">{0}</p>", content);

            return result;
        }

        public static string Blockquote(string content)
        {
            string result = string.Format("<blockquote>{0}</blockquote>", content);

            return result;
        }

        public static string Cite(string content)
        {
            string result = string.Format("<cite>{0}</cite>", content);

            return result;
        }

        public static string Strong(string content)
        {
            string result = string.Format("<strong>{0}</strong>", content);

            return result;
        }

        public static string Tag(string name, string content)
        {
            string result = string.Format("<{0}>{1}</{0}>", name, content);

            return result;
        }


        public static string GetHTMLA(string url, string text)
        {
            string el = string.Format("<a href='{0}'>{1}</a>", url, text);

            return el;
        }

        public static SmtpClient GetSMTPClient(out string login)
        {
            var appSettings = ConfigurationManager.AppSettings;

            string email = appSettings["e-mail"], password = appSettings["password"], host = appSettings["host"];
            int port = Convert.ToInt32(appSettings["port"]);
            bool enableSSL = Convert.ToBoolean(appSettings["enableSSL"]);

            var smtpClient = GetSMTPClient(email, password, host, port, enableSSL);

            login = email ?? (smtpClient.Credentials as NetworkCredential)?.UserName;

            return smtpClient;
        }

        public static SmtpClient GetSMTPClient(string login, string password, string host, int port, bool enableSsl)
        {
            if (!ObjectUtils.AreNotNull(login, password, host, port, enableSsl))
                return new SmtpClient();

            return new SmtpClient
            {
                Host = host, //"ssl0.ovh.net",
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(login, password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = enableSsl, //true
                Port = port //25
            };
        }
    }
}
