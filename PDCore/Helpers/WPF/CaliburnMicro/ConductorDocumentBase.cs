using Caliburn.Micro;
using PDCore.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace PDCore.Helpers.WPF.CaliburnMicro
{
    public class ConductorDocumentBase<TDocumentItem> : Conductor<TDocumentItem>.Collection.OneActive, IHaveShutdownTask
         where TDocumentItem : class, INotifyPropertyChanged, IDeactivate, IHaveDisplayName
    {
        bool isDirty;

        public bool IsDirty
        {
            get { return isDirty; }
            set
            {
                isDirty = value;
                NotifyOfPropertyChange(() => IsDirty);
            }
        }

        [Import]
        public IDialogManager Dialogs { get; set; }

        public override void CanClose(Action<bool> callback)
        {
            if (IsDirty)
                DoCloseCheck(Dialogs, callback);
            else
                callback(true);
        }

        public IResult GetShutdownTask()
        {
            return IsDirty ? new ApplicationCloseCheck(this, DoCloseCheck) : null;
        }

        protected virtual void DoCloseCheck(IDialogManager dialogs, Action<bool> callback)
        {
            dialogs.ShowMessageBox(
                "Masz niezapisane dane. Czy na pewno chcesz zamknąć ten dokument? Wszystkie zmiany zostaną utracone.",
                "Niezapisane dane",
                MessageBoxOptions.YesNo,
                box => callback(box.WasSelected(MessageBoxOptions.Yes))
                );
        }

        protected void DoCloseCheckWhenIsNotDirty(IDialogManager dialogs, Action<bool> callback)
        {
            dialogs.ShowMessageBox(
                "Czy na pewno chcesz zamknąć program?",
                "Zamknięcie programu",
                MessageBoxOptions.YesNo,
                box => callback(box.WasSelected(MessageBoxOptions.Yes))
                );
        }
    }
}
