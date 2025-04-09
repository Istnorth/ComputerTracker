using ComputerTracker.Data.DbModel;
using ComputerTracker.Data.Services;
using ComputerTracker.Pages;
using ComputerTracker.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ComputerTracker.Data.Pages
{
    public class EmployeesViewModel : INotifyPropertyChanged
    {
        private readonly EmployeeService _employeeService = new EmployeeService();

        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        public ICommand RefreshCommand { get; }
        public ICommand AddEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set { _selectedEmployee = value; OnPropertyChanged(); }
        }

        public EmployeesViewModel()
        {
            RefreshCommand = new RelayCommand(ExecuteRefresh);
            AddEmployeeCommand = new RelayCommand(ExecuteAddEmployee);
            EditEmployeeCommand = new RelayCommand(ExecuteEditEmployee, CanExecuteEmployeeCommand);
            DeleteEmployeeCommand = new RelayCommand(ExecuteDeleteEmployee, CanExecuteEmployeeCommand);
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            Employees.Clear();
            var employees = _employeeService.GetAllEmployees();
            foreach (var emp in employees)
                Employees.Add(emp);
        }

        private void ExecuteRefresh(object obj) => LoadEmployees();

        private void ExecuteAddEmployee(object obj)
        {
            var addVM = new AddEditEmployeeViewModel();
            var addWindow = new AddEditEmployeeWindow(addVM);
            if (Application.Current.MainWindow != null && Application.Current.MainWindow != addWindow)
            {
                addWindow.Owner = Application.Current.MainWindow;
            }

            if (addWindow.ShowDialog() == true)
            {
                var newEmployee = new Employee
                {
                    FirstName = addVM.FirstName,
                    LastName = addVM.LastName,
                    MiddleName = addVM.MiddleName,
                    DepartmentID = addVM.SelectedDepartment.DepartmentID
                };

                _employeeService.AddEmployee(newEmployee);
                LoadEmployees();
            }
        }

        private void ExecuteEditEmployee(object obj)
        {
            if (SelectedEmployee == null) return;

            var editVM = new AddEditEmployeeViewModel
            {
                FirstName = SelectedEmployee.FirstName,
                LastName = SelectedEmployee.LastName,
                MiddleName = SelectedEmployee.MiddleName,
                SelectedDepartment = SelectedEmployee.Department
            };

            var editWindow = new AddEditEmployeeWindow(editVM);
            if (Application.Current.MainWindow != null && Application.Current.MainWindow != editWindow)
            {
                editWindow.Owner = Application.Current.MainWindow;
            }

            if (editWindow.ShowDialog() == true)
            {
                SelectedEmployee.FirstName = editVM.FirstName;
                SelectedEmployee.LastName = editVM.LastName;
                SelectedEmployee.MiddleName = editVM.MiddleName;
                SelectedEmployee.DepartmentID = editVM.SelectedDepartment.DepartmentID;
                _employeeService.UpdateEmployee(SelectedEmployee);
                LoadEmployees();
            }
        }

        private void ExecuteDeleteEmployee(object obj)
        {
            if (SelectedEmployee == null) return;

            if (MessageBox.Show($"Удалить сотрудника {SelectedEmployee.FullName}?",
                                "Подтверждение удаления",
                                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _employeeService.DeleteEmployee(SelectedEmployee.EmployeeID);
                LoadEmployees();
            }
        }

        private bool CanExecuteEmployeeCommand(object obj) => SelectedEmployee != null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
