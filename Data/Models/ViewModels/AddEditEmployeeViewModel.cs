using ComputerTracker.Data.DbModel;
using ComputerTracker.Data.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ComputerTracker.Pages
{
    public class AddEditEmployeeViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }

        private string _middleName;
        public string MiddleName
        {
            get => _middleName;
            set { _middleName = value; OnPropertyChanged(); }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();

        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set { _selectedDepartment = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }

        public AddEditEmployeeViewModel()
        {
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            var departmentService = new DepartmentService();
            var departmentsFromDb = departmentService.GetAllDepartments();

            Departments.Clear();
            foreach (var dept in departmentsFromDb)
            {
                Departments.Add(dept);
            }
        }


        private void ExecuteSave(object parameter)
        {
            OnRequestClose(true);
        }

        private bool CanExecuteSave(object parameter)
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(MiddleName) &&
                   SelectedDepartment != null;
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
