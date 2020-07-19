namespace PDCore.Services.IServ
{
    public interface IMailService
    {
        void SendEmail(string receiverEmail, string title, string body, 
            string login = null, string password = null, string host = null, int port = 0, bool enableSsl = false);
    }
}