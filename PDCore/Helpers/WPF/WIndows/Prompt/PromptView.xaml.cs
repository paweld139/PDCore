using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PDCore.Helpers.WPF.Windows.Prompt
{
    /// <summary>
    /// Interaction logic for PromptView.xaml
    /// </summary>
    public partial class PromptView : Window
    {
        public PromptView()
        {
            InitializeComponent();
        }

        public PromptView(string displayName, string title = "", bool isPassword = false, params object[] data) : this()
        {
            var viewModel = new PromptViewModel(displayName, title, isPassword, data);

            viewModel.OnRequestClose += (s, e) => Close();


            DataContext = viewModel;
        }

        public string ResponseText
        {
            get
            {
                return (DataContext as PromptViewModel).ResponseText;
            }
        }
    }
}
