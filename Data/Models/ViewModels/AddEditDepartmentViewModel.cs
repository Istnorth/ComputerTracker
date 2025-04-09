using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ComputerTracker.Data.Pages
{
    public class AddEditDepartmentViewModel : INotifyPropertyChanged
    {
        private string _departmentName;
        public string DepartmentName
        {
            get => _departmentName;
            set { _departmentName = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }

        public AddEditDepartmentViewModel()
        {
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
        }

        private void ExecuteSave(object parameter)
        {
            OnRequestClose(true);
        }

        private bool CanExecuteSave(object parameter)
        {
            return !string.IsNullOrWhiteSpace(DepartmentName);
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