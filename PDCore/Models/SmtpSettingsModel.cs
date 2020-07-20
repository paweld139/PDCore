using PDCore.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace PDCore.Models
{
    public class SmtpSettingsModel
    {
        public SmtpSettingsModel()
        {
        }

        public SmtpSettingsModel(string login, string email, string displayName, string password, string host, int port, bool enableSsl)
        {
            Login = login;
            Email = email;
            DisplayName = displayName;
            Password = password.GetSecureString();
            Host = host;
            Port = port;
            EnableSsl = enableSsl;
        }

        public SmtpSettingsModel(NameValueCollection appSettings)
        {
            Login = appSettings["login"];
            Email = appSettings["email"];
            DisplayName = appSettings["displayName"];
            Password = appSettings["password"]?.GetSecureString();
            Host = appSettings["host"];
            Port = Convert.ToInt32(appSettings["port"]);
            EnableSsl = Convert.ToBoolean(appSettings["enableSSL"]);
        }

        public string Login { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public SecureString Password { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public bool EnableSsl { get; set; }


        public SmtpClient GetSmtpClient()
        {
            if (Email == null)
                return new SmtpClient();

            return new SmtpClient
            {
                Host = Host, //"ssl0.ovh.net",
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Login, Password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = EnableSsl, //true
                Port = Port //25
            };
        }
    }
}

