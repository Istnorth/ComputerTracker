using ComputerTracker.Data.Models.ViewModels;
using System.Windows.Controls;

namespace ComputerTracker.Pages
{
    public partial class ReportsPage : Page
    {
        public ReportsPage()
        {
            InitializeComponent();
            DataContext = new ReportsViewModel();
        }

    }
}
