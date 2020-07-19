using System.Threading.Tasks;

namespace PDCore.Services.IServ
{
    public interface IMailServiceAsync : IMailService
    {
        Task SendEmailAsync(string receiverEmail, string title, string body);
        Task SendEmailAsync(string receiverEmail, string title, string body, string login, string password, string host, int port, bool enableSsl);
    }
}