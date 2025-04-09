using ComputerTracker.Data.Models.ViewModels;
using System.Windows;

namespace ComputerTracker.Pages
{
    public partial class AddEditComputerWindow : Window
    {
        public AddEditComputerWindow(AddEditComputerViewModel viewModel)
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
