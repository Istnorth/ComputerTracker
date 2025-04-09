using ComputerTracker.Data.Models.ViewModels;
using System.Windows.Controls;

namespace ComputerTracker.Data.Pages
{
    /// <summary>
    /// Логика взаимодействия для ComputersPage.xaml
    /// </summary>
    public partial class ComputersPage : Page
    {
        public ComputersPage()
        {
            InitializeComponent();
            DataContext = new ComputersViewModel();
        }
    }
}
