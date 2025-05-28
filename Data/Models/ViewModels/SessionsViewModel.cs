using ComputerTracker.Data.DbModel;
using ComputerTracker.Data.Pages;
using ComputerTracker.Data.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ComputerTracker.Data.Models.ViewModels
{
    public class SessionsViewModel : INotifyPropertyChanged
    {
        private readonly SessionService _sessionService = new SessionService();

        public ObservableCollection<UsageSession> Sessions { get; set; } = new ObservableCollection<UsageSession>();

        public ICommand RefreshCommand { get; }
        public ICommand AddSessionCommand { get; }
        public ICommand EditSessionCommand { get; }
        public ICommand DeleteSessionCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand StartMonitoringCommand { get; }
        public ICommand StopMonitoringCommand { get; }
        public ICommand ShowAppUsageHistoryCommand { get; }
        public ICommand ShowKeyLogHistoryCommand { get; }

        private SoftwareUsageMonitor _softwareMonitor;

        private UsageSession _selectedSession;
        public UsageSession SelectedSession
        {
            get => _selectedSession;
            set { _selectedSession = value; OnPropertyChanged(); }
        }

        public SessionsViewModel()
        {
            RefreshCommand = new RelayCommand(ExecuteRefresh);
            AddSessionCommand = new RelayCommand(ExecuteAddSession);
            EditSessionCommand = new RelayCommand(ExecuteEditSession, CanExecuteSessionCommand);
            DeleteSessionCommand = new RelayCommand(ExecuteDeleteSession, CanExecuteSessionCommand);
            FilterCommand = new RelayCommand(ExecuteFilter);
            StartMonitoringCommand = new RelayCommand(ExecuteStartMonitoring, CanExecuteStartMonitoring);
            StopMonitoringCommand = new RelayCommand(ExecuteStopMonitoring, CanExecuteStopMonitoring);
            ShowAppUsageHistoryCommand = new RelayCommand(_ => ExecuteShowHistory(), _ => SelectedSession != null);
            ShowKeyLogHistoryCommand = new RelayCommand(_ => ExecuteShowKeyLogHistory(), _ => SelectedSession != null);

            LoadSessions();
        }

        private void LoadSessions()
        {
            Sessions.Clear();
            var sessions = _sessionService.GetAllSessions();
            foreach (var s in sessions)
                Sessions.Add(s);
        }

        private void ExecuteRefresh(object obj) => LoadSessions();

        private void ExecuteAddSession(object obj)
        {
            var addSessionVM = new AddEditSessionViewModel();
            var addSessionWindow = new AddEditSessionWindow(addSessionVM);
            if (Application.Current.MainWindow != null && Application.Current.MainWindow != addSessionWindow)
            {
                addSessionWindow.Owner = Application.Current.MainWindow;
            }

            if (addSessionWindow.ShowDialog() == true)
            {
                if (addSessionVM.SelectedEmployee == null || addSessionVM.SelectedComputer == null)
                {
                    MessageBox.Show("Выберите сотрудника и компьютер.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newSession = new UsageSession
                {
                    EmployeeID = addSessionVM.SelectedEmployee.EmployeeID,
                    ComputerID = addSessionVM.SelectedComputer.ComputerID,
                    StartTime = addSessionVM.StartTime,
                    EndTime = addSessionVM.EndTime,
                    Duration = addSessionVM.EndTime.HasValue
                              ? (int)(addSessionVM.EndTime.Value - addSessionVM.StartTime).TotalMinutes
                              : (int?)null
                };

                _sessionService.AddSession(newSession);
                LoadSessions();
            }
        }

        private void ExecuteEditSession(object obj)
        {
            if (SelectedSession == null) return;

            var editSessionVM = new AddEditSessionViewModel
            {
                SelectedEmployee = SelectedSession.Employee,
                SelectedComputer = SelectedSession.Computer,
                StartTime = SelectedSession.StartTime,
                EndTime = SelectedSession.EndTime
            };

            var editSessionWindow = new AddEditSessionWindow(editSessionVM);
            if (Application.Current.MainWindow != null && Application.Current.MainWindow !=editSessionWindow)
            {
                editSessionWindow.Owner = Application.Current.MainWindow;
            }

            if (editSessionWindow.ShowDialog() == true)
            {
                SelectedSession.EmployeeID = editSessionVM.SelectedEmployee.EmployeeID;
                SelectedSession.ComputerID = editSessionVM.SelectedComputer.ComputerID;
                SelectedSession.StartTime = editSessionVM.StartTime;
                SelectedSession.EndTime = editSessionVM.EndTime;
                SelectedSession.Duration = editSessionVM.EndTime.HasValue
                                            ? (int)(editSessionVM.EndTime.Value - editSessionVM.StartTime).TotalMinutes
                                            : (int?)null;
                SelectedSession.Employee = editSessionVM.SelectedEmployee;
                SelectedSession.Computer = editSessionVM.SelectedComputer;

                _sessionService.UpdateSession(SelectedSession);
                LoadSessions();
            }
        }

        private void ExecuteDeleteSession(object obj)
        {
            if (SelectedSession == null) return;

            if (MessageBox.Show("Вы уверены, что хотите удалить выбранную сессию?",
                                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _sessionService.DeleteSession(SelectedSession.SessionID);
                LoadSessions();
            }
        }

        private DateTime? _filterStartDate;
        public DateTime? FilterStartDate
        {
            get => _filterStartDate;
            set { _filterStartDate = value; OnPropertyChanged(); }
        }

        private DateTime? _filterEndDate;
        public DateTime? FilterEndDate
        {
            get => _filterEndDate;
            set { _filterEndDate = value; OnPropertyChanged(); }
        }


        private void ExecuteFilter(object obj)
        {
            var allSessions = _sessionService.GetAllSessions();

            if (FilterStartDate.HasValue)
            {
                DateTime startDateTime = FilterStartDate.Value.Date;
                allSessions = allSessions.Where(s => s.StartTime >= startDateTime).ToList();
            }
            if (FilterEndDate.HasValue)
            {
                DateTime endDateTime = FilterEndDate.Value.Date.AddDays(1).AddSeconds(-1);
                allSessions = allSessions.Where(s => s.StartTime <= endDateTime).ToList();
            }

            Sessions.Clear();
            foreach (var session in allSessions)
            {
                Sessions.Add(session);
            }
        }

        private void ExecuteShowHistory()
        {
            int computerId = SelectedSession.ComputerID;

            var historyVm = new AppUsageHistoryViewModel(computerId);
            var window = new AppUsageHistoryWindow { DataContext = historyVm };
            if (Application.Current.MainWindow != window)
                window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }

        private void ExecuteShowKeyLogHistory()
        {
            if (SelectedSession == null) return;

            var vm = new KeyLogHistoryViewModel(SelectedSession.ComputerID);

            var wnd = new KeyLogHistoryWindow
            {
                DataContext = vm,
                Owner = Application.Current.MainWindow
            };
            wnd.ShowDialog();
        }

        private bool CanExecuteSessionCommand(object obj) => SelectedSession != null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        private bool CanExecuteStartMonitoring(object obj)
        {
            return SelectedSession != null && _softwareMonitor == null;
        }
        private void ExecuteStartMonitoring(object obj)
        {
            _softwareMonitor = new SoftwareUsageMonitor(SelectedSession.SessionID);
            _softwareMonitor.Start();
        }
        private bool CanExecuteStopMonitoring(object obj)
        {
            return _softwareMonitor != null;
        }
        private void ExecuteStopMonitoring(object obj)
        {
            _softwareMonitor?.Stop();
            _softwareMonitor = null;
        }
    }
}
