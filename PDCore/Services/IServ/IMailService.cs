using PDCore.Models;

namespace PDCore.Services.IServ
{
    public interface IMailService
    {
        void SendEmail(MailMessageModel mailMessageModel, SmtpSettingsModel smtpSettingsModel = null);
    }
}