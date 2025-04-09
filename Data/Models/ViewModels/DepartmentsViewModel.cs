using ComputerTracker.Data.DbModel;
using ComputerTracker.Data.Pages;
using ComputerTracker.Data.Services;
using ComputerTracker.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ComputerTracker.Data.Models.ViewModels
{
    public class DepartmentsViewModel : INotifyPropertyChanged
    {
        private readonly DepartmentService _departmentService = new DepartmentService();

        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();

        public ICommand RefreshCommand { get; }
        public ICommand AddDepartmentCommand { get; }
        public ICommand EditDepartmentCommand { get; }
        public ICommand DeleteDepartmentCommand { get; }

        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set { _selectedDepartment = value; OnPropertyChanged(); }
        }

        public DepartmentsViewModel()
        {
            RefreshCommand = new RelayCommand(ExecuteRefresh);
            AddDepartmentCommand = new RelayCommand(ExecuteAddDepartment);
            EditDepartmentCommand = new RelayCommand(ExecuteEditDepartment, CanExecuteDepartmentCommand);
            DeleteDepartmentCommand = new RelayCommand(ExecuteDeleteDepartment, CanExecuteDepartmentCommand);
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            Departments.Clear();
            var departments = _departmentService.GetAllDepartments();
            foreach (var dept in departments)
            {
                Departments.Add(dept);
            }
        }

        private void ExecuteRefresh(object obj) => LoadDepartments();

        private void ExecuteAddDepartment(object obj)
        {
            var addVM = new AddEditDepartmentViewModel();
            var addWindow = new AddEditDepartmentWindow(addVM);
            if (Application.Current.MainWindow != null && Application.Current.MainWindow != addWindow)
            {
                addWindow.Owner = Application.Current.MainWindow;
            }

            if (addWindow.ShowDialog() == true)
            {
                var newDept = new Department
                {
                    DepartmentName = addVM.DepartmentName
                };

                _departmentService.AddDepartment(newDept);
                LoadDepartments();
            }
        }

        private void ExecuteEditDepartment(object obj)
        {
            if (SelectedDepartment == null)
                return;

            var editVM = new AddEditDepartmentViewModel
            {
                DepartmentName = SelectedDepartment.DepartmentName
            };

            var editWindow = new AddEditDepartmentWindow(editVM);
            if (Application.Current.MainWindow != null && Application.Current.MainWindow != editWindow)
            {
                editWindow.Owner = Application.Current.MainWindow;
            }

            if (editWindow.ShowDialog() == true)
            {
                SelectedDepartment.DepartmentName = editVM.DepartmentName;
                _departmentService.UpdateDepartment(SelectedDepartment);
                LoadDepartments();
            }
        }

        private void ExecuteDeleteDepartment(object obj)
        {
            if (SelectedDepartment == null)
                return;

            if (MessageBox.Show($"Удалить отдел \"{SelectedDepartment.DepartmentName}\"?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _departmentService.DeleteDepartment(SelectedDepartment.DepartmentID);
                LoadDepartments();
            }
        }

        private bool CanExecuteDepartmentCommand(object obj) => SelectedDepartment != null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}