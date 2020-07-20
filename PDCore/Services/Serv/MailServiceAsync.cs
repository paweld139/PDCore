using PDCore.Interfaces;
using PDCore.Models;
using PDCore.Services.IServ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Unity;

namespace PDCore.Services.Serv
{
    public class MailServiceAsync : MailService, IMailServiceAsync
    {
        public MailServiceAsync(SmtpSettingsModel smtpSettingsModel, ILogger logger) : base(smtpSettingsModel, logger)
        {
        }

        [InjectionConstructor]
        public MailServiceAsync(ILogger logger) : base(logger)
        {
        }

        public void SendEmailAsync(MailMessageModel mailMessageModel, SmtpSettingsModel smtpSettingsModel = null)
        {
            var data = GetData(mailMessageModel, smtpSettingsModel);

            SendEmailAsync(data.Item1, data.Item2);
        }

        public void SendEmailAsync(MailMessage message, SmtpSettingsModel smtpSettingsModel = null)
        {
            var client = GetSmtpClient(smtpSettingsModel);

            SendEmailAsync(message, client);
        }

        public void SendEmailAsync(MailMessage message)
        {
            SendEmailAsync(message, new SmtpClient());
        }

        public void SendEmailAsync(MailMessage message, SmtpClient client)
        {
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
