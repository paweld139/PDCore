using System.Windows;
using System.Windows.Controls;

namespace PDCore.Helpers.WPF.Windows.WebViewer
{
    /// <summary>
    /// Interaction logic for WebViewerView.xaml
    /// </summary>
    public partial class WebViewerView : Window
    {
        public WebViewerView()
        {
            InitializeComponent();
        }

        public WebViewerView(string displayName, string content, bool landscape = false) : this()
        {
            if (landscape)
            {
                WebBrowser.Height = 600;

                WebBrowser.Width = 800;
            }

            var viewModel = new WebViewerViewModel(displayName);

            viewModel.OnRequestClose += (s, e) => Close();


            DataContext = viewModel;


            WebBrowser.NavigateToString(content);
        }
    }
}
