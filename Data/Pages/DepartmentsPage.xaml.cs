    using ComputerTracker.Data.Models.ViewModels;
    using System.Windows.Controls;

    namespace ComputerTracker.Data.Pages
    {
        /// <summary>
        /// Логика взаимодействия для DepartmentsPage.xaml
        /// </summary>
        public partial class DepartmentsPage : Page
        {
            public DepartmentsPage()
            {
                InitializeComponent();
                DataContext = new DepartmentsViewModel();
            }
        }
    }
