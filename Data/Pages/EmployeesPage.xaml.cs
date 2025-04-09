using ComputerTracker.Data.Pages;
using System.Windows.Controls;

namespace ComputerTracker.Data.Pages
{
    /// <summary>
    /// Логика взаимодействия для EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        public EmployeesPage()
        {
            InitializeComponent();
            DataContext = new EmployeesViewModel();
        }
    }
}
