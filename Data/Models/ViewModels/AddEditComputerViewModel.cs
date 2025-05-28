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

        private string _host;
        public string Host
        {
            get => _host;
            set { _host = value; OnPropertyChanged(); }
        }

        private int _port;
        public int Port
        {
            get => _port;
            set { _port = value; OnPropertyChanged(); }
        }

        private string _cpuName;
        public string CPUName
        {
            get => _cpuName;
            set { _cpuName = value; OnPropertyChanged(); }
        }

        private int _cpuCores;
        public int CpuCores
        {
            get => _cpuCores;
            set { _cpuCores = value; OnPropertyChanged(); }
        }

        private int _cpuThreads;
        public int CpuThreads
        {
            get => _cpuThreads;
            set { _cpuThreads = value; OnPropertyChanged(); }
        }

        private int _cpuClockMHz;
        public int CpuClockMHz
        {
            get => _cpuClockMHz;
            set { _cpuClockMHz = value; OnPropertyChanged(); }
        }

        private string _osCaption;
        public string OSCaption
        {
            get => _osCaption;
            set { _osCaption = value; OnPropertyChanged(); }
        }

        private string _osVersion;
        public string OSVersion
        {
            get => _osVersion;
            set
            {
                _osVersion = value; OnPropertyChanged();
            }
        }

        private string _osManufacturer;
        public string OSManufacturer
        {
            get => _osManufacturer;
            set { _osManufacturer = value; OnPropertyChanged(); }
        }

        private string _windowsDirectory;
        public string WindowsDirectory
        {
            get => _windowsDirectory;
            set { _windowsDirectory = value; OnPropertyChanged(); }
        }

        // Команда сохранения и событие закрытия
        public ICommand SaveCommand { get; }
        public event EventHandler<bool> RequestClose;

        public AddEditComputerViewModel()
        {
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
        }

        private void ExecuteSave(object parameter)
        {
            RequestClose?.Invoke(this, true);
        }

        private bool CanExecuteSave(object parameter)
        {
            return !string.IsNullOrWhiteSpace(ComputerName)
                   && !string.IsNullOrWhiteSpace(IPAddress)
                   && !string.IsNullOrWhiteSpace(Host)
                   && Port > 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
