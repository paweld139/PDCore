using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Commands.WPF
{
    public class RelayCommand2 : System.Windows.Input.ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommand2(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke() ?? false;
        }

        public void Execute(object parameter)
        {
            execute?.Invoke();
        }

        public event EventHandler CanExecuteChanged
        {
            add { System.Windows.Input.CommandManager.RequerySuggested += value; }
            remove { System.Windows.Input.CommandManager.RequerySuggested -= value; }
        }

        public void RaiseCanExecuteChanged()
        {
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
        }
    }
}
