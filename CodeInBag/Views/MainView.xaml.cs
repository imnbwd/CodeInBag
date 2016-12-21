using CodeInBag.ViewModels;
using System.Windows.Controls;

namespace CodeInBag.Views
{
    public partial class MainView : UserControl
    {
        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}