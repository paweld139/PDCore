using Microsoft.VisualBasic.Logging;
using PDCore.Enums;
using PDCore.Interfaces;
using PDCore.Services.IServ;
using PDCore.Utils;
using System;
using System.ComponentModel;
using System.Net.Mail;

namespace PDCore.Services.Serv
{
    public class MailService : IMailService
    {
        private const string SendCompletedMessageFormat = "{0} email to {1} with subject [{2}]";

        private readonly string login;
        private readonly string password;
        private readonly string host;
        private readonly int port;
        private readonly bool enableSsl;

        public MailService(string login, string password, string host, int port, bool enableSsl, ILogger logger) : this(logger)
        {
            this.login = login;
            this.password = password;
            this.host = host;
            this.port = port;
            this.enableSsl = enableSsl;
        }

        private readonly ILogger logger;

        public MailService(ILogger logger)
        {
            this.logger = logger;
        }

        private SmtpClient GetSmtpClient(string login, string password, string host, int port, bool enableSsl, MailMessage mailMessage)
        {
            SmtpClient smtpClient;

            bool areNotNull = ObjectUtils.AreNotNull(login, password, host, port, enableSsl);
            bool areNotNullThis = ObjectUtils.AreNotNull(this.login, this.password, this.host, this.port, this.enableSsl);

            if (areNotNull)
            {
                smtpClient = WebUtils.GetSMTPClient(login, password, host, port, enableSsl);
            }
            else if (areNotNullThis)
            {
                smtpClient = WebUtils.GetSMTPClient(this.login, this.password, this.host, this.port, this.enableSsl);

                login = this.login;
            }
            else
            {
                smtpClient = WebUtils.GetSMTPClient(out login);
            }

            mailMessage.From = new MailAddress(login);


            return smtpClient;
        }

        public void SendEmail(string receiverEmail, string title, string body)
        {
            SendEmail(receiverEmail, title, body, null, null, null, 0, false);
        }

        public void SendEmail(string receiverEmail, string title, string body, string login, string password, string host, int port, bool enableSsl)
        {
            var data = PrepareSending(receiverEmail, title, body, login, password, host, port, enableSsl);

            var client = data.Item1;

            var message = data.Item2;

            client.SendCompleted += (s, e) =>
            {
                SendCompletedCallback(s, e);

                client.Dispose();
                message.Dispose();
            };

            try
            {
                client.SendAsync(message, message);

                logger.Info(string.Format(SendCompletedMessageFormat, "Sending async", message.To, message.Subject));
            }
            catch (Exception ex)
            {
                logger.Fatal("Async email error", ex);
            }
        }

        protected Tuple<SmtpClient, MailMessage> PrepareSending(string receiverEmail, string title, string body, string login, string password, string host, int port, bool enableSsl)
        {
            MailMessage message = new MailMessage
            {
                Subject = title,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(receiverEmail);


            SmtpClient client = GetSmtpClient(login, password, host, port, enableSsl, message);


            return new Tuple<SmtpClient, MailMessage>(client, message);
        }

        protected void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            var mail = (MailMessage)e.UserState;

            if (e.Error != null)
            {
                logger.Error(string.Format(SendCompletedMessageFormat, "Error sending", mail.To, mail.Subject), e.Error);
            }
            else if (e.Cancelled)
            {
                logger.Warn(string.Format(SendCompletedMessageFormat, "Cancelled", mail.To, mail.Subject));
            }
            else
            {
                logger.Info(string.Format(SendCompletedMessageFormat, "Sent email", mail.To, mail.Subject));
            }
        }
    }
}
