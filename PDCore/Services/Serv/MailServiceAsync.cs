using PDCore.Interfaces;
using PDCore.Services.IServ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace PDCore.Services.Serv
{
    public class MailServiceAsync : MailService, IMailServiceAsync
    {
        public MailServiceAsync(ILogger logger) : base(logger)
        {
        }

        public MailServiceAsync(string login, string password, string host, int port, bool enableSsl, ILogger logger) : base(login, password, host, port, enableSsl, logger)
        {
        }

        public void SendEmailAsync(string receiverEmail, string title, string body)
        {
            SendEmailAsync(receiverEmail, title, body, null, null, null, 0, false);
        }

        public void SendEmailAsync(string receiverEmail, string title, string body, string login, string password, string host, int port, bool enableSsl)
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

                logger.Info(string.Format(SendStatusMessageFormat, "Sending async", message.To, message.Subject));
            }
            catch (Exception ex)
            {
                logger.Fatal("Async email error", ex);
            }
        }

        protected void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            var mail = (MailMessage)e.UserState;

            if (e.Error != null)
            {
                logger.Error(string.Format(SendStatusMessageFormat, "Error sending", mail.To, mail.Subject), e.Error);
            }
            else if (e.Cancelled)
            {
                logger.Warn(string.Format(SendStatusMessageFormat, "Cancelled", mail.To, mail.Subject));
            }
            else
            {
                logger.Info(string.Format(SendStatusMessageFormat, "Sent email", mail.To, mail.Subject));
            }
        }
    }
}
