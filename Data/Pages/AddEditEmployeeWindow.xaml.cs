using System.Windows;

namespace ComputerTracker.Pages
{
    public partial class AddEditEmployeeWindow : Window
    {
        public AddEditEmployeeWindow(AddEditEmployeeViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.RequestClose += (s, dialogResult) =>
            {
                DialogResult = dialogResult;
                Close();
            };
        }
        public AddEditEmployeeWindow()
        {
            InitializeComponent();
        }
    }
}
