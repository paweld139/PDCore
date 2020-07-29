using System.Windows.Controls;

namespace PDCore.Helpers.WPF.CaliburnMicro
{
    public interface IWorkspace
    {
        string Icon { get; }
        string IconName { get; set; }
        string Status { get; set; }
        bool DisplayExpand { get; set; }
        bool IsExpanded { get; set; }
        void Show();
        ContextMenu DisplayDetails { get; set; }
        void Initialize();
    }
}
