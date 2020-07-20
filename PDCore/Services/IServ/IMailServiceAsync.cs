using PDCore.Models;
using System.Threading.Tasks;

namespace PDCore.Services.IServ
{
    public interface IMailServiceAsync : IMailService
    {
        void SendEmailAsync(MailMessageModel mailMessageModel, SmtpSettingsModel smtpSettingsModel = null);
    }
}