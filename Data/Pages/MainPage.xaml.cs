using ComputerTracker.Data.Models.ViewModels;
using ComputerTracker.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ComputerTracker.Data.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        public MainPage()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            MainFrame.Navigate(new ComputersPage());
        }

        private void NavigationListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (ListBoxItem)((ListBox)sender).SelectedItem;
            if (selectedItem == null) return;
            switch (selectedItem.Content.ToString())
            {
                case "Компьютеры":
                    MainFrame.Navigate(new ComputersPage());
                    break;
                case "Сессии":
                    MainFrame.Navigate(new SessionsPage());
                    break;
                case "Отчёты":
                    MainFrame.Navigate(new ReportsPage());
                    break;
                case "Сотрудники":
                    MainFrame.Navigate(new EmployeesPage());
                    break;
                case "Отделы":
                    MainFrame.Navigate(new DepartmentsPage());
                    break;
                default:
                    break;
            }
        }
    }
}
