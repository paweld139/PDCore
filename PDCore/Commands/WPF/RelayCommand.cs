﻿using System;
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

        public event EventHandler CanExecuteChanged;

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
            return canExecute?.Invoke(Parameter ?? parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            execute?.Invoke(Parameter ?? parameter);
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}