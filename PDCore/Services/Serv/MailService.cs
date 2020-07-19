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


            client.SendAsync(message, message);
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

            client.SendCompleted += (s, e) =>
            {
                OnSendAsyncCompleted(s, e);

                client.Dispose();
                message.Dispose();
            };

            return new Tuple<SmtpClient, MailMessage>(client, message);
        }

        protected void OnSendAsyncCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var mail = (MailMessage)e.UserState;

            //write out the subject
            string subject = mail.Subject;

            if (e.Cancelled)
            {
                logger?.Log(string.Format("Send canceled for mail with subject [{0}].", subject), LogType.Warn);
            }
            if (e.Error != null)
            {
                logger?.Log(string.Format("Error occurred when sending mail [{0}] ", subject), e.Error, LogType.Error);
            }
            else
            {
                logger?.Log(string.Format("Message [{0}] sent.", subject), LogType.Info);
            }
        }
    }
}
