﻿using Org.BouncyCastle.Crypto.Tls;
using PDCore.Interfaces;
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
    public class MailServiceAsync : MailService
    {
        public MailServiceAsync(string login, string password, string host, int port, bool enableSsl, ILogger logger) : 
            base(login, password, host, port, enableSsl, logger)
        {
        }

        public MailServiceAsync(ILogger logger) : base(logger)
        {
        }

        public async Task SendEmailAsync(string receiverEmail, string title, string body, string login = null, string password = null, string host = null, int port = 0, bool enableSsl = false)
        {
            var data = PrepareSending(receiverEmail, title, body, login, password, host, port, enableSsl);
            
            
            using (var message = data.Item2)
            {
                using (var client = data.Item1)
                {
                    Task sendMailTask = null;

                    try
                    {
                        sendMailTask = client.SendMailAsync(message);

                        await sendMailTask;
                    }
                    catch
                    {
                    }

                    AsyncCompletedEventArgs args = new AsyncCompletedEventArgs(sendMailTask.Exception, sendMailTask.IsCanceled, message);

                    OnSendAsyncCompleted(this, args);
                }
            }
        }
    }
}
