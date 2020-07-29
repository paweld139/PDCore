using System;
using System.Net.Sockets;
using System.Windows.Forms;

namespace PDCore.Helpers.TCP
{
    public interface ITCPManager
    {
        bool Reset { get; set; }
        bool Started { get; set; }
        void DoBeginAcceptTcpClient(TcpListener listener);
        void SetText(string text, object target, Form parent);
        void Listen(string port2, bool actualIp, string ipFrom);
        void DoAcceptTcpClientCallback(IAsyncResult ar);
        string GetLocalIPAddress();
        string SendMessage(string port1, string ip, string message);
        TcpListener Server { get; set; }
    }
}
