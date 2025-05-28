using ComputerTracker.Data.DbModel;
using ComputerTracker.Data.Services;
using ComputerTracker.Pages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ComputerTracker.Data.Models.ViewModels
{
    public class ComputersViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Computer> Computers { get; }
        public ICommand RefreshCommand { get; }
        public ICommand AddComputerCommand { get; }
        public ICommand EditComputerCommand { get; }
        public ICommand DeleteComputerCommand { get; }
        public ICommand UpdateSystemDataCommand { get; }

        private Computer _selectedComputer;
        public Computer SelectedComputer
        {
            get => _selectedComputer;
            set { _selectedComputer = value; OnPropertyChanged(); }
        }

        private readonly ComputerService _computerService = new ComputerService();

        public ComputersViewModel()
        {
            Computers = new ObservableCollection<Computer>();
            RefreshCommand = new RelayCommand(_ => LoadComputers());
            AddComputerCommand = new RelayCommand(_ => ExecuteAdd());
            EditComputerCommand = new RelayCommand(_ => ExecuteEdit(), _ => SelectedComputer != null);
            DeleteComputerCommand = new RelayCommand(_ => ExecuteDelete(), _ => SelectedComputer != null);
            UpdateSystemDataCommand = new RelayCommand(_ => ExecuteUpdateSystemData(), _ => SelectedComputer != null);

            LoadComputers();
        }

        private void LoadComputers()
        {
            Computers.Clear();
            using var ctx = new AppDbContext();
            foreach (var c in ctx.Computers
                                 .Include(c => c.SystemData)
                                 .Include(c => c.Gpus)
                                 .Include(c => c.Keyboards)
                                 .Include(c => c.Mice)
                                 .Include(c => c.Printers)
                                 .Include(c => c.Scanners)
                                 .Include(c => c.Monitors))
            {
                Computers.Add(c);
            }
        }

        private void ExecuteAdd()
        {
            try
            {
                var vm = new AddEditComputerViewModel();
                var win = new AddEditComputerWindow(vm) { Owner = Application.Current.MainWindow };
                if (win.ShowDialog() != true) return;

                var comp = new Computer
                {
                    ComputerName = vm.ComputerName,
                    IPAddress = vm.IPAddress,
                    Host = vm.Host,
                    Port = vm.Port,
                    LastUpdated = DateTime.UtcNow
                };
                _computerService.AddComputer(comp);

                // создаём первую запись SystemData сразу:
                var sys = new ComputerSystemData
                {
                    ComputerID = comp.ComputerID,
                    Timestamp = DateTime.UtcNow,
                    OSCaption = vm.OSCaption,
                    OSVersion = vm.OSVersion,
                    OSManufacturer = vm.OSManufacturer,
                    WindowsDirectory = vm.WindowsDirectory,
                    CpuCores = vm.CpuCores,
                    CpuThreads = vm.CpuThreads,
                    CpuClockMHz = vm.CpuClockMHz
                };
                using var ctx = new AppDbContext();
                ctx.ComputerSystemDatas.Add(sys);
                ctx.SaveChanges();

                LoadComputers();
            }
            catch (DbUpdateException dbex)
            {
                var sqlEx = dbex.GetBaseException();
                MessageBox.Show(
                    $"SaveChanges failed:\n{dbex.Message}\n\nInner: {sqlEx.Message}",
                    "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteEdit()
        {
            try
            {
                if (SelectedComputer == null) return;

                // заполняем VM из выбранного
                var vm = new AddEditComputerViewModel
                {
                    ComputerName = SelectedComputer.ComputerName,
                    IPAddress = SelectedComputer.IPAddress,
                    Host = SelectedComputer.Host,
                    Port = SelectedComputer.Port,
                    OSCaption = SelectedComputer.LatestSystemData?.OSCaption,
                    OSVersion = SelectedComputer.LatestSystemData?.OSVersion,
                    OSManufacturer = SelectedComputer.LatestSystemData?.OSManufacturer,
                    WindowsDirectory = SelectedComputer.LatestSystemData?.WindowsDirectory,
                    CpuCores = SelectedComputer.LatestSystemData?.CpuCores ?? 0,
                    CpuThreads = SelectedComputer.LatestSystemData?.CpuThreads ?? 0,
                    CpuClockMHz = SelectedComputer.LatestSystemData?.CpuClockMHz ?? 0
                };
                var win = new AddEditComputerWindow(vm) { Owner = Application.Current.MainWindow };
                if (win.ShowDialog() != true) return;

                // обновляем свойства компьютера
                SelectedComputer.ComputerName = vm.ComputerName;
                SelectedComputer.IPAddress = vm.IPAddress;
                SelectedComputer.Host = vm.Host;
                SelectedComputer.Port = vm.Port;
                SelectedComputer.LastUpdated = DateTime.UtcNow;
                _computerService.UpdateComputer(SelectedComputer);

                // создаём новую запись SystemData
                var sys = new ComputerSystemData
                {
                    ComputerID = SelectedComputer.ComputerID,
                    Timestamp = DateTime.UtcNow,
                    OSCaption = vm.OSCaption,
                    OSVersion = vm.OSVersion,
                    OSManufacturer = vm.OSManufacturer,
                    WindowsDirectory = vm.WindowsDirectory,
                    CpuCores = vm.CpuCores,
                    CpuThreads = vm.CpuThreads,
                    CpuClockMHz = vm.CpuClockMHz
                };
                using var ctx = new AppDbContext();
                ctx.ComputerSystemDatas.Add(sys);
                ctx.SaveChanges();

                LoadComputers();
            }
            catch (DbUpdateException dbex)
            {
                var sqlEx = dbex.GetBaseException();
                MessageBox.Show(
                    $"SaveChanges failed:\n{dbex.Message}\n\nInner: {sqlEx.Message}",
                    "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteDelete()
        {
            if (SelectedComputer == null) return;
            if (MessageBox.Show(
                    $"Удалить компьютер «{SelectedComputer.ComputerName}»?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            _computerService.DeleteComputer(SelectedComputer.ComputerID);
            LoadComputers();
        }

        private void ExecuteUpdateSystemData()
        {
            try
            {
                new SystemDataService().UpdateSystemData(SelectedComputer.ComputerID);
                LoadComputers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка обновления системных данных:\n{ex}\n\nInner:\n{ex.InnerException}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
