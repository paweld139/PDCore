using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Commands
{
    public interface ICommand
    {
        void Execute();
        bool CanExecute();
        void Undo();
    }
}
