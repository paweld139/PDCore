using Microsoft.VisualBasic.Logging;
using PDCore.Enums;
using PDCore.Interfaces;
using PDCore.Services.IServ;
using PDCore.Utils;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;

namespace PDCore.Services.Serv
{
    public class MailService : IMailService
    {
        protected const string SendStatusMessageFormat = "{0} email to {1} with subject [{2}]";

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

        protected readonly ILogger logger;

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

        public void SendEmail(string receiverEmail, string title, string body)
        {
            SendEmail(receiverEmail, title, body, null, null, null, 0, false);
        }

        public void SendEmail(string receiverEmail, string title, string body, string login, string password, string host, int port, bool enableSsl)
        {
            var data = PrepareSending(receiverEmail, title, body, login, password, host, port, enableSsl);

            var client = data.Item1;

            var message = data.Item2;

            try
            {
                logger.Info(string.Format(SendStatusMessageFormat, "Sending sync", message.To, message.Subject));

                client.Send(message);              
            }
            catch (Exception ex)
            {
                logger.Fatal("Sync email error", ex);
            }
        }
    }
}
