using Org.BouncyCastle.Crypto.Tls;
using PDCore.Interfaces;
using PDCore.Models;
using PDCore.Services.IServ;
using PDCore.Services.Serv;
using PDCore.Utils;
using PDCoreNew.Helpers.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace PDCoreNew.Services.Serv
{
    public class MailServiceAsyncTask : MailServiceAsync, IMailServiceAsyncTask
    {
        public MailServiceAsyncTask(SmtpSettingsModel smtpSettingsModel, ILogger logger) : base(smtpSettingsModel, logger)
        {
        }

        [InjectionConstructor]
        public MailServiceAsyncTask(ILogger logger) : base(logger)
        {
        }

        public Task SendEmailAsyncTask(MailMessageModel mailMessageModel, SmtpSettingsModel smtpSettingsModel = null)
        {
            var data = GetData(mailMessageModel, smtpSettingsModel);

            return SendEmailAsyncTask(data.Item1, data.Item2);
        }

        public Task SendEmailAsyncTask(MailMessage message, SmtpSettingsModel smtpSettingsModel = null)
        {
            var client = GetSmtpClient(smtpSettingsModel);

            return SendEmailAsyncTask(message, client);
        }

        public Task SendEmailAsyncTask(MailMessage message)
        {
            return SendEmailAsyncTask(message, new SmtpClient());
        }

        public async Task SendEmailAsyncTask(MailMessage message, SmtpClient client)
        {
            using (message)
            {
                using (client)
                {
                    try
                    {
                        Task sendMailTask = client.SendMailAsync(message);


                        logger.Info(string.Format(SendStatusMessageFormat, "Sending async", message.To, message.Subject));


                        await sendMailTask;


                        AsyncCompletedEventArgs args = new AsyncCompletedEventArgs(sendMailTask?.Exception, sendMailTask?.IsCanceled ?? false, message);

                        SendCompletedCallback(this, args);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("Async email error", ex);
                    }
                }
            }
        }
    }
}
