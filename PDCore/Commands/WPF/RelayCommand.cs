using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Commands.WPF
{
    public class RelayCommand : System.Windows.Input.ICommand
    {
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        public object Parameter { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { System.Windows.Input.CommandManager.RequerySuggested += value; }
            remove { System.Windows.Input.CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, object parameter = null)
        {
            this.execute = execute;
            Parameter = parameter;
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute, object parameter = null) : this(execute, parameter)
        {
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter ?? Parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            execute?.Invoke(parameter ?? Parameter);
        }

        public void FireCanExecuteChanged()
        {
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
        }
    }
}
