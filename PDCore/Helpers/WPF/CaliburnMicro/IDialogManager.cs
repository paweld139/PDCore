using System;
using Caliburn.Micro;
using PDCore.Enums;
using System.Windows;
using MessageBoxOptions = PDCore.Enums.MessageBoxOptions;

namespace PDCore.Helpers.WPF.CaliburnMicro
{
    public interface IDialogManager
    {
        void ShowDialog(IScreen dialogModel, VerticalAlignment verticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center);
        void ShowMessageBox(string message, string title = null, MessageBoxOptions options = MessageBoxOptions.Ok,
            Action<IMessageBox> callback = null, VerticalAlignment verticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center);
    }
}
