using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic.Logging;
using PDCore.Enums;
using PDCore.Extensions;
using PDCore.Interfaces;
using PDCore.Models;
using PDCore.Services.IServ;
using PDCore.Utils;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace PDCore.Services.Serv
{
    public class MailService : IMailService
    {
        protected const string SendStatusMessageFormat = "{0} email to {1} with subject [{2}]";

        private readonly SmtpSettingsModel smtpSettingsModel;

        public MailService(SmtpSettingsModel smtpSettingsModel, ILogger logger) : this(logger)
        {
            this.smtpSettingsModel = smtpSettingsModel;
        }

        protected readonly ILogger logger;

        public MailService(ILogger logger)
        {
            this.logger = logger;
        }

        protected Tuple<SmtpClient, MailMessage> PrepareSending(MailMessageModel mailMessageModel, SmtpSettingsModel smtpSettingsModel)
        {
            if (this.smtpSettingsModel != null) // Zostało przekazane proprzez konstruktor
            {
                smtpSettingsModel = this.smtpSettingsModel;
            }
            else if (smtpSettingsModel == null) // Nie zostało przekazane proprzez konstruktor i poprzez metodę
            {
                var appSettings = ConfigurationManager.AppSettings;

                smtpSettingsModel = new SmtpSettingsModel(appSettings);
            }

            var message = mailMessageModel.GetMailMessage(smtpSettingsModel);

            var client = smtpSettingsModel.GetSmtpClient();

            
            return new Tuple<SmtpClient, MailMessage>(client, message);
        }

        public void SendEmail(MailMessageModel mailMessageModel, SmtpSettingsModel smtpSettingsModel = null)
        {
            var data = PrepareSending(mailMessageModel, smtpSettingsModel);

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
