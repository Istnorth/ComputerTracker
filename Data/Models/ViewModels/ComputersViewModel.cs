using ComputerTracker.Data.DbModel;
using ComputerTracker.Data.Services;
using ComputerTracker.Pages;
using DocumentFormat.OpenXml.Drawing.Charts;
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
        public ObservableCollection<Computer> Computers { get; set; }

        public ICommand RefreshCommand { get; }
        public ICommand AddComputerCommand { get; }
        public ICommand EditComputerCommand { get; }
        public ICommand DeleteComputerCommand { get; }
        public ICommand UpdateSystemDataCommand { get; }

        private Computer _selectedComputer;
        public Computer SelectedComputer
        {
            get => _selectedComputer;
            set
            {
                _selectedComputer = value;
                OnPropertyChanged();
            }
        }

        private readonly ComputerService _computerService = new ComputerService();

        public ComputersViewModel()
        {
            Computers = new ObservableCollection<Computer>();
            RefreshCommand = new RelayCommand(ExecuteRefresh);
            AddComputerCommand = new RelayCommand(ExecuteAdd);
            EditComputerCommand = new RelayCommand(ExecuteEdit, CanExecuteEdit);
            DeleteComputerCommand = new RelayCommand(ExecuteDelete, CanExecuteDelete);
            UpdateSystemDataCommand = new RelayCommand(ExecuteUpdateSystemData, CanExecuteUpdateSystemData);
            LoadComputers();
        }

        private void LoadComputers()
        {
            using (var context = new AppDbContext())
            {
                Computers.Clear();
                foreach (var comp in context.Computers.Include("SystemData"))
                {
                    Computers.Add(comp);
                }
            }
        }

        private void ExecuteRefresh(object obj) => LoadComputers();

        private void ExecuteAdd(object obj)
        {
            var addVM = new AddEditComputerViewModel();
            var addWindow = new AddEditComputerWindow(addVM);

            if (Application.Current.MainWindow != null && Application.Current.MainWindow != addWindow)
            {
                addWindow.Owner = Application.Current.MainWindow;
            }

            if (addWindow.ShowDialog() == true)
            {
                var newComputer = new Computer
                {
                    ComputerName = addVM.ComputerName,
                    IPAddress = addVM.IPAddress,
                    LastUpdated = DateTime.Now
                };

                _computerService.AddComputer(newComputer);

                bool systemDataProvided = addVM.CPUUsage != 0 ||
                                          addVM.MemoryUsage != 0 ||
                                          addVM.DiskUsage != 0 ||
                                          addVM.NetworkUsage != 0 ||
                                          !string.IsNullOrWhiteSpace(addVM.OSVersion);

                if (systemDataProvided)
                {
                    using (var context = new AppDbContext())
                    {
                        var computerInDb = context.Computers.Find(newComputer.ComputerID);
                        if (computerInDb != null)
                        {
                            var systemData = new ComputerSystemData
                            {
                                ComputerID = computerInDb.ComputerID,
                                Timestamp = DateTime.Now,
                                CPUUsage = addVM.CPUUsage,
                                MemoryUsage = addVM.MemoryUsage,
                                DiskUsage = addVM.DiskUsage,
                                NetworkUsage = addVM.NetworkUsage,
                                OSVersion = addVM.OSVersion
                            };

                            context.ComputerSystemDatas.Add(systemData);
                            context.SaveChanges();
                        }
                    }
                }
                LoadComputers();
            }
        }

        private void ExecuteEdit(object obj)
        {
            if (SelectedComputer == null) return;
            var editVM = new AddEditComputerViewModel
            {
                ComputerName = SelectedComputer.ComputerName,
                IPAddress = SelectedComputer.IPAddress,

                CPUUsage = SelectedComputer.LatestSystemData?.CPUUsage ?? 0,
                MemoryUsage = SelectedComputer.LatestSystemData?.MemoryUsage ?? 0,
                DiskUsage = SelectedComputer.LatestSystemData?.DiskUsage ?? 0,
                NetworkUsage = SelectedComputer.LatestSystemData?.NetworkUsage ?? 0,
                OSVersion = SelectedComputer.LatestSystemData?.OSVersion ?? string.Empty
            };

            var editWindow = new AddEditComputerWindow(editVM);
            if (Application.Current.MainWindow != null && Application.Current.MainWindow != editWindow)
            {
                editWindow.Owner = Application.Current.MainWindow;
            }

            if (editWindow.ShowDialog() == true)
            {
                SelectedComputer.ComputerName = editVM.ComputerName;
                SelectedComputer.IPAddress = editVM.IPAddress;
                SelectedComputer.LastUpdated = DateTime.Now;

                bool systemDataChanged = editVM.CPUUsage != (SelectedComputer.LatestSystemData?.CPUUsage ?? 0)
                                         || editVM.MemoryUsage != (SelectedComputer.LatestSystemData?.MemoryUsage ?? 0)
                                         || editVM.DiskUsage != (SelectedComputer.LatestSystemData?.DiskUsage ?? 0)
                                         || editVM.NetworkUsage != (SelectedComputer.LatestSystemData?.NetworkUsage ?? 0)
                                         || editVM.OSVersion != (SelectedComputer.LatestSystemData?.OSVersion ?? string.Empty);

                if (systemDataChanged)
                {
                    using (var context = new AppDbContext())
                    {
                        var comp = context.Computers
                                          .Include(c => c.SystemData)
                                          .FirstOrDefault(c => c.ComputerID == SelectedComputer.ComputerID);
                        if (comp != null)
                        {
                            var newSysData = new ComputerSystemData
                            {
                                ComputerID = comp.ComputerID,
                                Timestamp = DateTime.Now,
                                CPUUsage = editVM.CPUUsage,
                                MemoryUsage = editVM.MemoryUsage,
                                DiskUsage = editVM.DiskUsage,
                                NetworkUsage = editVM.NetworkUsage,
                                OSVersion = editVM.OSVersion
                            };
                            context.ComputerSystemDatas.Add(newSysData);
                            context.Computers.Update(comp);
                            context.SaveChanges();
                        }
                    }
                }
                _computerService.UpdateComputer(SelectedComputer);
                LoadComputers();
            }
        }


        private bool CanExecuteEdit(object obj) => SelectedComputer != null;

        private void ExecuteDelete(object obj)
        {
            if (SelectedComputer == null) return;

            var result = MessageBox.Show($"Удалить компьютер \"{SelectedComputer.ComputerName}\" и все связанные системные данные?",
                                         "Подтверждение удаления",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _computerService.DeleteComputer(SelectedComputer.ComputerID);
                LoadComputers();
            }
        }
        private bool CanExecuteDelete(object obj) => SelectedComputer != null;

        private bool CanExecuteUpdateSystemData(object obj) => SelectedComputer != null;

        private void ExecuteUpdateSystemData(object obj)
        {
            try
            {
                var systemDataService = new SystemDataService();
                systemDataService.UpdateSystemData(SelectedComputer.ComputerID);
                MessageBox.Show("Системные данные обновлены", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadComputers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления системных данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
