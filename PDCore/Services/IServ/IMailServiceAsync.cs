using System.Threading.Tasks;

namespace PDCore.Services.IServ
{
    public interface IMailServiceAsync : IMailService
    {
        void SendEmailAsync(string receiverEmail, string title, string body);
        void SendEmailAsync(string receiverEmail, string title, string body, string login, string password, string host, int port, bool enableSsl);
    }
}