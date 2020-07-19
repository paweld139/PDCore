namespace PDCore.Services.IServ
{
    public interface IMailService
    {
        void SendEmail(string receiverEmail, string title, string body);
        void SendEmail(string receiverEmail, string title, string body, string login, string password, string host, int port, bool enableSsl);
    }
}