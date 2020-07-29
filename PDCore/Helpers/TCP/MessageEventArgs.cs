using System;

namespace PDCore.Helpers.TCP
{
    public class MessageEventArgs : EventArgs
    {
        public string Text { get; private set; }

        public MessageEventArgs(string text)
        {
            Text = text;
        }
    }
}
