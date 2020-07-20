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
    public class MailServiceAsync : MailService, IMailServiceAsync
    {
        public MailServiceAsync(string login, string password, string host, int port, bool enableSsl, ILogger logger) :
            base(login, password, host, port, enableSsl, logger)
        {
        }

        public MailServiceAsync(ILogger logger) : base(logger)
        {
        }

        public Task SendEmailAsync(string receiverEmail, string title, string body)
        {
            return SendEmailAsync(receiverEmail, title, body, null, null, null, 0, false);
        }

        public async Task SendEmailAsync(string receiverEmail, string title, string body, string login, string password, string host, int port, bool enableSsl)
        {
            var data = PrepareSending(receiverEmail, title, body, login, password, host, port, enableSsl);


            using (var message = data.Item2)
            {
                using (var client = data.Item1)
                {
                    var result = await ActionWrapper.ExecuteAsync(() => client.SendMailAsync(message));

                    Task sendMailTask = result.Item3;

                    Exception exception = result.Item2;

                    AsyncCompletedEventArgs args = new AsyncCompletedEventArgs(sendMailTask?.Exception ?? exception, sendMailTask?.IsCanceled ?? false, message);

                    SendCompletedCallback(this, args);
                }
            }
        }
    }
}
