using ComputerTracker.Data.DbModel;
using ComputerTracker.Data.Services;
using ComputerTracker.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ComputerTracker.Data.Models.ViewModels
{
    public class ReportsViewModel : INotifyPropertyChanged
    {
        private readonly ReportService _reportService = new ReportService();
        private readonly ComputerService _computerService = new ComputerService();

        public ObservableCollection<object> ReportItems { get; set; } = new ObservableCollection<object>();

        private DateTime _reportStartDate = DateTime.Now.AddDays(-7);
        public DateTime ReportStartDate
        {
            get => _reportStartDate;
            set { _reportStartDate = value; OnPropertyChanged(); }
        }

        private DateTime _reportEndDate = DateTime.Now;
        public DateTime ReportEndDate
        {
            get => _reportEndDate;
            set { _reportEndDate = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> ReportTypes { get; set; } = new ObservableCollection<string> { "Активность", "Компьютер" };

        private string _reportType = "Активность";
        public string ReportType
        {
            get => _reportType;
            set { _reportType = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Computer> Computers { get; set; } = new ObservableCollection<Computer>();

        private Computer _selectedComputer;
        public Computer SelectedComputer
        {
            get => _selectedComputer;
            set { _selectedComputer = value; OnPropertyChanged(); }
        }

        public ICommand GenerateReportCommand { get; }
        public ICommand ExportReportCommand { get; }

        public ReportsViewModel()
        {
            GenerateReportCommand = new RelayCommand(ExecuteGenerateReport);
            ExportReportCommand = new RelayCommand(ExecuteExportReport);
            LoadComputers();
        }

        private void LoadComputers()
        {
            Computers.Clear();
            var comps = _computerService.GetAllComputers();
            foreach (var comp in comps)
                Computers.Add(comp);
        }

        private void ExecuteGenerateReport(object obj)
        {
            ReportItems.Clear();

            if (ReportType == "Компьютер")
            {
                if (SelectedComputer == null)
                {
                    ReportItems.Add("Выберите компьютер для формирования отчета.");
                    return;
                }
                var report = _reportService.GetComputerReport(SelectedComputer.ComputerID, ReportStartDate, ReportEndDate);
                ReportItems.Add(report);
            }
            else if (ReportType == "Активность")
            {
                var items = _reportService.GetActivityReport(ReportStartDate, ReportEndDate);
                foreach (var item in items)
                {
                    ReportItems.Add(item);
                }
            }
        }

        private void ExecuteExportReport(object obj)
        {
            if (!ReportItems.Any()) return;

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filePath = $"Отчет_{ReportType}_{timestamp}.xlsx";

            if (ReportType == "Активность")
            {
                var typedList = ReportItems.OfType<ActivityReportItem>().ToList();
                _reportService.ExportActivityReport(typedList, filePath);
            }
            else if (ReportType == "Компьютер")
            {
                var compReport = ReportItems.OfType<ComputerReport>().FirstOrDefault();
                if (compReport != null)
                {
                    _reportService.ExportComputerReport(compReport, filePath);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
