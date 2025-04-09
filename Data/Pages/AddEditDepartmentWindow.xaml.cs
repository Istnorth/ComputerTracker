using System.Windows;

namespace ComputerTracker.Data.Pages
{
    public partial class AddEditDepartmentWindow : Window
    {
        public AddEditDepartmentWindow(AddEditDepartmentViewModel viewModel)
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