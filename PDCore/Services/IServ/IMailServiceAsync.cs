using PDCore.Models;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PDCore.Services.IServ
{
    public interface IMailServiceAsync : IMailService
    {
        void SendEmailAsync(MailMessageModel mailMessageModel, SmtpSettingsModel smtpSettingsModel = null);

        void SendEmailAsync(MailMessage message, SmtpSettingsModel smtpSettingsModel = null);

        void SendEmailAsync(MailMessage message);

        void SendEmailAsync(MailMessage message, SmtpClient client);
    }
}