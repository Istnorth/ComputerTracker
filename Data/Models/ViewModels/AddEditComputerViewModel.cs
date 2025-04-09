using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ComputerTracker.Data.Models.ViewModels
{
    public class AddEditComputerViewModel : INotifyPropertyChanged
    {
        private string _computerName;
        public string ComputerName
        {
            get => _computerName;
            set { _computerName = value; OnPropertyChanged(); }
        }

        private string _ipAddress;
        public string IPAddress
        {
            get => _ipAddress;
            set { _ipAddress = value; OnPropertyChanged(); }
        }

        private double _cpuUsage;
        public double CPUUsage
        {
            get => _cpuUsage;
            set { _cpuUsage = value; OnPropertyChanged(); }
        }

        private double _memoryUsage;
        public double MemoryUsage
        {
            get => _memoryUsage;
            set { _memoryUsage = value; OnPropertyChanged(); }
        }

        private double _diskUsage;
        public double DiskUsage
        {
            get => _diskUsage;
            set { _diskUsage = value; OnPropertyChanged(); }
        }

        private double _networkUsage;
        public double NetworkUsage
        {
            get => _networkUsage;
            set { _networkUsage = value; OnPropertyChanged(); }
        }

        private string _osVersion;
        public string OSVersion
        {
            get => _osVersion;
            set { _osVersion = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }

        public AddEditComputerViewModel()
        {
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
        }

        private void ExecuteSave(object parameter)
        {
            OnRequestClose(true);
        }

        private bool CanExecuteSave(object parameter)
        {
            return !string.IsNullOrWhiteSpace(ComputerName) &&
                   !string.IsNullOrWhiteSpace(IPAddress);
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
