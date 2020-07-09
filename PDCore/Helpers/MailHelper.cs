using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace PDCore.Helpers
{
    public class MailHelper
    {
        private static SmtpClient _smtpServer = null;
        public static SmtpClient SMTPServer
        {
            get
            {
                if (_smtpServer == null)
                {
                    InitialiseSMTPClient();
                }

                return _smtpServer;
            }
        }

        public const string EmailTitle = "";

        private static void InitialiseSMTPClient()
        {
            InitialiseSMTPClient(ConfigurationManager.AppSettings["e-mail"], ConfigurationManager.AppSettings["password"]);
        }

        public static void InitialiseSMTPClient(string login, string password)
        {
            _smtpServer = new SmtpClient
            {
                Host = "ssl0.ovh.net",
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(login, password),
                EnableSsl = true,
                Port = 25
            };
        }

        public static bool SendEmail(string receiverEmail, string title, string body)
        {
            return SendEmail(receiverEmail, title, body, (SMTPServer.Credentials as NetworkCredential).UserName);
        }

        public static bool SendEmail(string receiverEmail, string title, string body, string login, string password = null)
        {
            try
            {
                if (password != null && _smtpServer == null)
                {
                    InitialiseSMTPClient(login, password);
                }

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(login)
                };

                mail.To.Add(receiverEmail);
                mail.Subject = title;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SMTPServer.Send(mail);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Dispose()
        {
            SMTPServer.Dispose();
            _smtpServer = null;
        }
    }
}
