using Org.BouncyCastle.Crypto.Tls;
using PDCore.Interfaces;
using PDCore.Services.IServ;
using PDCore.Services.Serv;
using PDCore.Utils;
using PDCoreNew.Helpers.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Services.Serv
{
    public class MailServiceAsyncTask : MailServiceAsync, IMailServiceAsyncTask
    {
        public MailServiceAsyncTask(string login, string password, string host, int port, bool enableSsl, ILogger logger) :
            base(login, password, host, port, enableSsl, logger)
        {
        }

        public MailServiceAsyncTask(ILogger logger) : base(logger)
        {
        }

        public Task SendEmailAsyncTask(string receiverEmail, string title, string body)
        {
            return SendEmailAsyncTask(receiverEmail, title, body, null, null, null, 0, false);
        }

        public async Task SendEmailAsyncTask(string receiverEmail, string title, string body, string login, string password, string host, int port, bool enableSsl)
        {
            var data = PrepareSending(receiverEmail, title, body, login, password, host, port, enableSsl);


            using (var message = data.Item2)
            {
                using (var client = data.Item1)
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
