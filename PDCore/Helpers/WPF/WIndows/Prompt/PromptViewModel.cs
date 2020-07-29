using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace PDCore.Helpers.WPF.Windows.Prompt
{
    public class PromptViewModel //: INotifyPropertyChanged
    {
        public event EventHandler OnRequestClose;


        public string ResponseText { get; set; }

        public bool IsPassword { get; private set; }

        public string Title { get; private set; }

        public string DisplayName { get; private set; }

        public object[] Data { get; private set; }


        public PromptViewModel(string displayName, string title = "", bool isPassword = false, params object[] data)
        {
            DisplayName = displayName;

            Title = title;

            Data = data;

            IsPassword = isPassword;
        }

        public PromptViewModel()
        {
            DisplayName = "Wpisz tekst";
        }

        private ICommand _okCommand;
        public ICommand OkCommand
        {
            get
            {
                if (_okCommand == null)
                {
                    _okCommand = new PDCore.Commands.WPF.RelayCommand(a =>
                    {
                        Ok(a);
                    },
                    w =>
                    {
                        return true;
                    });
                }

                return _okCommand;
            }
        }

        public void Ok(object obj)
        {
            if (IsPassword)
            {
                ResponseText = ((PasswordBox)obj).Password;
            }

            OnRequestClose(this, new EventArgs());
        }
    }
}
