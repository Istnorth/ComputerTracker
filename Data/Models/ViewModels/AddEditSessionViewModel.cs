using ComputerTracker.Data.DbModel;
using ComputerTracker.Data.Services;
using ComputerTracker.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ComputerTracker.Data.Models.ViewModels
{
    public class AddEditSessionViewModel : INotifyPropertyChanged
    {
        private readonly EmployeeService _employeeService = new EmployeeService();
        private readonly ComputerService _computerService = new ComputerService();

        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<Computer> Computers { get; set; } = new ObservableCollection<Computer>();

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set { _selectedEmployee = value; OnPropertyChanged(); }
        }

        private Computer _selectedComputer;
        public Computer SelectedComputer
        {
            get => _selectedComputer;
            set { _selectedComputer = value; OnPropertyChanged(); }
        }

        private DateTime _startTime = DateTime.Now;
        public DateTime StartTime
        {
            get => _startTime;
            set { _startTime = value; OnPropertyChanged(); }
        }

        private DateTime? _endTime;
        public DateTime? EndTime
        {
            get => _endTime;
            set { _endTime = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }

        public AddEditSessionViewModel()
        {
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
            LoadEmployees();
            LoadComputers();
        }

        private void LoadEmployees()
        {
            var employees = _employeeService.GetAllEmployees();
            Employees.Clear();
            foreach (var emp in employees)
                Employees.Add(emp);
        }

        private void LoadComputers()
        {
            var computers = _computerService.GetAllComputers();
            Computers.Clear();
            foreach (var comp in computers)
                Computers.Add(comp);
        }

        private void ExecuteSave(object parameter)
        {
            OnRequestClose(true);
        }

        private bool CanExecuteSave(object parameter)
        {
            return SelectedEmployee != null && SelectedComputer != null && StartTime != default;
        }

        public event EventHandler<bool> RequestClose;
        protected void OnRequestClose(bool dialogResult)
        {
            RequestClose?.Invoke(this, dialogResult);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
