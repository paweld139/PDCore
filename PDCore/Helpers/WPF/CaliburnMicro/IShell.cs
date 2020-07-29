using Caliburn.Micro;

namespace PDCore.Helpers.WPF.CaliburnMicro
{
    public interface IShell : IConductor, IGuardClose
    {
        IDialogManager Dialogs { get; }
    }
}
