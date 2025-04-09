using ComputerTracker.Data.Pages.LogRegPages;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ComputerTracker.Data.Models.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _currentUserName;
        public string CurrentUserName
        {
            get => _currentUserName;
            set
            {
                if (_currentUserName != value)
                {
                    _currentUserName = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LogoutCommand { get; }

        public MainViewModel()
        {
            LogoutCommand = new RelayCommand(ExecuteLogout, CanExecuteLogout);
        }

        private void ExecuteLogout(object parameter)
        {
            var loginWindow = new LoginWindow();
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            loginWindow.Show();
            Application.Current.MainWindow = loginWindow;

            foreach (Window window in Application.Current.Windows)
            {
                if (window != loginWindow)
                {
                    window.Close();
                }
            }
        }


        private bool CanExecuteLogout(object parameter)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}