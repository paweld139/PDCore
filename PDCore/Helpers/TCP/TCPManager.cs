using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PDCore.Helpers.TCP
{
    public class TCPManager : ITCPManager
    {
        public bool Reset { get; set; }
        public bool Started { get; set; }
        public string MsgId { get; set; }
        public string Error { get; set; }

        public delegate void MessageEventHandler(object sender, MessageEventArgs e);
        public delegate string SendResponse(string msgId, string error);
        public delegate void NotifyEventHandler(string message);

        public event MessageEventHandler SetReceivedText;
        public event MessageEventHandler SetResponseText;
        public event SendResponse SendResponseText;
        public event NotifyEventHandler Notify;

        public ManualResetEvent TcpClientConnected { get; set; }

        public void DoBeginAcceptTcpClient(TcpListener listener)
        {
            // Set the event to nonsignaled state.
            TcpClientConnected.Reset();

            // Accept the connection. 
            // BeginAcceptSocket() creates the accepted socket.
            listener.BeginAcceptTcpClient(
                new AsyncCallback(DoAcceptTcpClientCallback),
                listener);
            // Wait until a connection is made and processed before 
            // continuing.
            TcpClientConnected.WaitOne();
        }

        public void SetText(string text, object target, Form parent)
        {
            if (parent.InvokeRequired)
                parent.Invoke((EventHandler)(delegate (object sender, EventArgs e)
                {
                    SetText(text, target, parent);
                }));
            else
            {
                if (target is TextBox)
                    (target as TextBox).Text = text;
                else
                    target = text;
            }
        }

        public void StopListen()
        {
            if (Started)
            {

                Started = false;
                Server.Stop();

                TcpClientConnected.Reset();
                TcpClientConnected.Close();
                TcpClientConnected.Dispose();
            }
        }

        public TcpListener Server { get; set; }

        public void Listen(string port2, bool actualIp, string ipFrom)
        {

            if (port2 == "" || !int.TryParse(port2, out int f))
            {
                Notify?.Invoke("Podano nieprawidłowy port nasłuchujący");

                return;
            }

            try
            {
                if (!Started)
                {
                    Server = null;
                    TcpClientConnected = new ManualResetEvent(false);
                    int port = Convert.ToInt32(port2);
                    IPAddress localAddr = IPAddress.Parse(actualIp ? GetLocalIPAddress() : ipFrom);

                    Server = new TcpListener(localAddr, port);

                    // Start listening for client requests.
                    Server.Start();

                    Started = true;

                    Notify?.Invoke("Włączono nasłuchiwanie");

                    // Enter the listening loop.
                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    DoBeginAcceptTcpClient(Server);
                }
                else
                {
                    Notify?.Invoke("Nasłuchiwanie zostało włączone ponownie");

                    Reset = true;
                    Server.Stop();
                    int port = Convert.ToInt32(port2);
                    IPAddress localAddr = IPAddress.Parse(GetLocalIPAddress());

                    Server = new TcpListener(localAddr, port);

                    // Start listening for client requests.
                    Server.Start();

                    // Enter the listening loop.
                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    DoBeginAcceptTcpClient(Server);
                }
            }
            catch (SocketException m)
            {
                Notify?.Invoke(m.Message.ToString());
            }
        }

        public void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            if (Reset)
            {
                Reset = false;
                return;
            }
            // Get the listener that handles the client request.
            TcpListener listener = (TcpListener)ar.AsyncState;

            // End the operation and display the received data on 
            // the console.

            TcpClient client = null;

            try
            {
                client = listener.EndAcceptTcpClient(ar);
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            // Buffer for reading data
            byte[] bytes = new byte[2560];
            string data = null;

            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            int i;

            // Loop to receive all the data sent by the client.
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                data = Encoding.UTF8.GetString(bytes, 0, i);

                Notify?.Invoke("Otrzymano komunikat o treści:" + Environment.NewLine + data);

                SetReceivedText?.Invoke(this, new MessageEventArgs(data));

                if (SendResponseText != null)
                {
                    string response = SendResponseText(MsgId, Error);

                    byte[] msg = Encoding.UTF8.GetBytes(response);

                    // Send back a response.
                    stream.Write(msg, 0, msg.Length);


                    Notify?.Invoke("Wysłano response o treści:" + Environment.NewLine + response);
                }
            }

            bytes = null;

            stream.Close();
            client.Close();

            //GC.Collect();

            listener.BeginAcceptTcpClient(
               new AsyncCallback(DoAcceptTcpClientCallback),
               listener);

            // Signal the calling thread to continue.
        }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            Notify?.Invoke("no network adapters with an ipv4 address in the system!");

            return "";
        }

        public string SendMessage(string port1, string ip, string message)
        {

            if (port1 == "" || !int.TryParse(port1, out int g))
            {
                if (ip == "")
                {
                    Notify?.Invoke("Podano nieprawidłowy port do wysłania i nieprawidłowe ip");

                    return null;
                }

                Notify?.Invoke("Podano nieprawidłowy port do wysłania");

                return null;
            }
            else if (ip == "")
            {
                Notify?.Invoke("Podano nieprawidłowe ip");

                return null;
            }
            else if (message == "")
            {
                Notify?.Invoke("Nie podano wiadomości do wysłania");

                return null;
            }

            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                int port = Convert.ToInt32(port1);
                TcpClient client = new TcpClient(ip, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                byte[] data = Encoding.UTF8.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();


                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Notify?.Invoke("Wysłano komunikat o treści:" + Environment.NewLine + message);

                //// Receive the TcpServer.response.

                //// Buffer to store the response bytes.
                data = new byte[2560];

                // String to store the response ASCII representation.
                string responseData = string.Empty;

                //// Read the first batch of the TcpServer response bytes.
                int bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.UTF8.GetString(data, 0, bytes);

                SetResponseText?.Invoke(this, new MessageEventArgs(responseData));

                data = null;

                stream.Close();
                client.Close();

                return responseData;
            }
            catch (ArgumentNullException m)
            {
                Notify?.Invoke(m.Message.ToString());

                return null;
            }
            catch (SocketException n)
            {
                Notify?.Invoke(n.Message.ToString());

                return null;
            }
        }
    }
}
