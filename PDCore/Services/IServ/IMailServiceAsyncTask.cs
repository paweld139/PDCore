using PDCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Services.IServ
{
    public interface IMailServiceAsyncTask : IMailServiceAsync
    {
        Task SendEmailAsyncTask(MailMessageModel mailMessageModel, SmtpSettingsModel smtpSettingsModel = null);

        Task SendEmailAsyncTask(MailMessage message, SmtpSettingsModel smtpSettingsModel = null);

        Task SendEmailAsyncTask(MailMessage message);

        Task SendEmailAsyncTask(MailMessage message, SmtpClient client);
    }
}
