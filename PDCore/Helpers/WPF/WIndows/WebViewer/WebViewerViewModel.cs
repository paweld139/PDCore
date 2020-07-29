using mshtml;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace PDCore.Helpers.WPF.Windows.WebViewer
{
    public class WebViewerViewModel //: INotifyPropertyChanged
    {
        public event EventHandler OnRequestClose;


        public string DisplayName { get; private set; }


        public WebViewerViewModel(string displayName)
        {
            DisplayName = displayName;
        }

        public WebViewerViewModel()
        {
            DisplayName = "Podgląd strony";
        }

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new PDCore.Commands.WPF.RelayCommand(a =>
                    {
                        Close(a);
                    },
                    w =>
                    {
                        return true;
                    });
                }

                return _closeCommand;
            }
        }

        private ICommand _printCommand;
        public ICommand PrintCommand
        {
            get
            {
                if (_printCommand == null)
                {
                    _printCommand = new PDCore.Commands.WPF.RelayCommand(a =>
                    {
                        Print(a);
                    },
                    w =>
                    {
                        return true;
                    });
                }

                return _printCommand;
            }
        }

        private void Close(object obj)
        {
            ((WebBrowser)obj).Dispose();

            OnRequestClose(this, new EventArgs());
        }

        private void Print(object obj)
        {
            IHTMLDocument2 doc = ((WebBrowser)obj).Document as IHTMLDocument2;

            doc.execCommand("Print", true, null);
        }
    }
}
