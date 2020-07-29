using Caliburn.Micro;
using System;
using System.ComponentModel.Composition;

namespace PDCore.Helpers.WPF.CaliburnMicro
{
    public class ApplicationCloseCheck : IResult
    {
        readonly Action<IDialogManager, Action<bool>> closeCheck;
        readonly IChild screen;

        public ApplicationCloseCheck(IChild screen, Action<IDialogManager, Action<bool>> closeCheck)
        {
            this.screen = screen;
            this.closeCheck = closeCheck;
        }

        [Import]
        public IShell Shell { get; set; }

        public void Execute(CoroutineExecutionContext context)
        {
            if (screen.Parent is IDocumentWorkspace documentWorkspace)
                documentWorkspace.Edit(screen);

            closeCheck(Shell.Dialogs, result => Completed(this, new ResultCompletionEventArgs { WasCancelled = !result }));
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };
    }
}
