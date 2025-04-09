using ComputerTracker.Data.Models.ViewModels;
using System.Windows.Controls;

namespace ComputerTracker.Data.Pages
{
    /// <summary>
    /// Логика взаимодействия для SessionsPage.xaml
    /// </summary>
    public partial class SessionsPage : Page
    {
        public SessionsPage()
        {
            InitializeComponent();
            this.DataContext = new SessionsViewModel();
        }
    }
}
