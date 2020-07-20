using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Services.IServ
{
    public interface IMailServiceAsyncTask : IMailServiceAsync
    {
        Task SendEmailAsyncTask(string receiverEmail, string title, string body);
        Task SendEmailAsyncTask(string receiverEmail, string title, string body, string login, string password, string host, int port, bool enableSsl);
    }
}
