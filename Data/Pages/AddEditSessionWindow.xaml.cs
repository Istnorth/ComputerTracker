using ComputerTracker.Data.Models.ViewModels;
using System.Windows;
using Xceed.Wpf.Toolkit;

namespace ComputerTracker.Data.Pages
{
    public partial class AddEditSessionWindow : Window
    {
        public AddEditSessionWindow(AddEditSessionViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.RequestClose += (s, dialogResult) =>
            {
                DialogResult = dialogResult;
                Close();
            };
        }
    }
}
