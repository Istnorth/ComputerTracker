using ComputerTracker.Data.DbModel;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputerTracker.Services
{
    public class ReportService
    {
        public ComputerReport GetComputerReport(int computerId, DateTime? periodStart = null, DateTime? periodEnd = null)
        {
            using (var context = new AppDbContext())
            {
                var computer = context.Computers
                    .Include(c => c.SystemData)
                    .Include(c => c.UsageSessions)
                        .ThenInclude(s => s.Employee)
                    .FirstOrDefault(c => c.ComputerID == computerId);

                if (computer == null)
                    throw new Exception("Компьютер не найден");

                var systemData = computer.SystemData.AsQueryable();
                if (periodStart.HasValue) systemData = systemData.Where(sd => sd.Timestamp >= periodStart.Value);
                if (periodEnd.HasValue) systemData = systemData.Where(sd => sd.Timestamp <= periodEnd.Value);

                var sessions = computer.UsageSessions.AsQueryable();
                if (periodStart.HasValue) sessions = sessions.Where(s => s.StartTime >= periodStart.Value);
                if (periodEnd.HasValue) sessions = sessions.Where(s => s.StartTime <= periodEnd.Value);

                return new ComputerReport
                {
                    ComputerID = computer.ComputerID,
                    ComputerName = computer.ComputerName,
                    IPAddress = computer.IPAddress,
                    LastUpdated = computer.LastUpdated,
                    SystemData = systemData.OrderByDescending(sd => sd.Timestamp).ToList(),
                    UsageSessions = sessions.OrderBy(s => s.StartTime).ToList()
                };
            }
        }

        public List<ActivityReportItem> GetActivityReport(DateTime periodStart, DateTime periodEnd)
        {
            using (var context = new AppDbContext())
            {
                var sessions = context.UsageSessions
                    .Include(s => s.Computer)
                    .Where(s => s.StartTime >= periodStart && s.StartTime <= periodEnd)
                    .ToList();

                var reportItems = sessions
                    .GroupBy(s => s.Computer)
                    .Select(g => new ActivityReportItem
                    {
                        ComputerID = g.Key.ComputerID,
                        ComputerName = g.Key.ComputerName,
                        TotalDurationMinutes = g.Sum(s => s.Duration ?? 0),
                        SessionCount = g.Count()
                    })
                    .ToList();

                return reportItems;
            }
        }

        public void ExportComputerReport(ComputerReport report, string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                // 1) Лист с основной информацией о компьютере
                var infoSheet = workbook.Worksheets.Add("ComputerInfo");
                infoSheet.Cell(1, 1).Value = "Номер компьютера";
                infoSheet.Cell(1, 2).Value = "Имя компьютера";
                infoSheet.Cell(1, 3).Value = "IP адрес";
                infoSheet.Cell(1, 4).Value = "Последнее обновление";

                infoSheet.Cell(2, 1).Value = report.ComputerID;
                infoSheet.Cell(2, 2).Value = report.ComputerName;
                infoSheet.Cell(2, 3).Value = report.IPAddress;
                infoSheet.Cell(2, 4).Value = report.LastUpdated.ToString("yyyy-MM-dd HH:mm:ss");

                // 2) Лист с сессиями
                var sessionSheet = workbook.Worksheets.Add("UsageSessions");
                sessionSheet.Cell(1, 1).Value = "Номер сессии";
                sessionSheet.Cell(1, 2).Value = "Номер сотрудника";
                sessionSheet.Cell(1, 3).Value = "Начало";
                sessionSheet.Cell(1, 4).Value = "Конец";
                sessionSheet.Cell(1, 5).Value = "Длительность(мин)";

                for (int i = 0; i < report.UsageSessions.Count; i++)
                {
                    var s = report.UsageSessions[i];
                    int row = i + 2;
                    sessionSheet.Cell(row, 1).Value = s.SessionID;
                    sessionSheet.Cell(row, 2).Value = s.EmployeeID;
                    sessionSheet.Cell(row, 3).Value = s.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    sessionSheet.Cell(row, 4).Value = s.EndTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
                    sessionSheet.Cell(row, 5).Value = s.Duration ?? 0;
                }

                // 3) Лист с системными данными
                var sysDataSheet = workbook.Worksheets.Add("SystemData");
                sysDataSheet.Cell(1, 1).Value = "ID записи";
                sysDataSheet.Cell(1, 2).Value = "Время фиксации";
                sysDataSheet.Cell(1, 3).Value = "OS Caption";
                sysDataSheet.Cell(1, 4).Value = "OS Version";
                sysDataSheet.Cell(1, 5).Value = "OS Manufacturer";
                sysDataSheet.Cell(1, 6).Value = "Windows Directory";
                sysDataSheet.Cell(1, 7).Value = "CPU Cores";
                sysDataSheet.Cell(1, 8).Value = "CPU Threads";
                sysDataSheet.Cell(1, 9).Value = "CPU Clock (MHz)";

                for (int i = 0; i < report.SystemData.Count; i++)
                {
                    var sd = report.SystemData[i];
                    int row = i + 2;
                    sysDataSheet.Cell(row, 1).Value = sd.SystemDataID;
                    sysDataSheet.Cell(row, 2).Value = sd.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                    sysDataSheet.Cell(row, 3).Value = sd.OSCaption;
                    sysDataSheet.Cell(row, 4).Value = sd.OSVersion;
                    sysDataSheet.Cell(row, 5).Value = sd.OSManufacturer;
                    sysDataSheet.Cell(row, 6).Value = sd.WindowsDirectory;
                    sysDataSheet.Cell(row, 7).Value = sd.CpuCores;
                    sysDataSheet.Cell(row, 8).Value = sd.CpuThreads;
                    sysDataSheet.Cell(row, 9).Value = sd.CpuClockMHz;
                }

                workbook.SaveAs(filePath);
            }
        }

        public void ExportActivityReport(List<ActivityReportItem> reportItems, string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var sheet = workbook.Worksheets.Add("ActivityReport");
                sheet.Cell(1, 1).Value = "Номер компьютера";
                sheet.Cell(1, 2).Value = "Имя компьютера";
                sheet.Cell(1, 3).Value = "Длительность(мин)";
                sheet.Cell(1, 4).Value = "Кол-во сессий";

                for (int i = 0; i < reportItems.Count; i++)
                {
                    var item = reportItems[i];
                    int row = i + 2;
                    sheet.Cell(row, 1).Value = item.ComputerID;
                    sheet.Cell(row, 2).Value = item.ComputerName;
                    sheet.Cell(row, 3).Value = item.TotalDurationMinutes;
                    sheet.Cell(row, 4).Value = item.SessionCount;
                }

                workbook.SaveAs(filePath);
            }
        }
    }

    public class ComputerReport
    {
        public int ComputerID { get; set; }
        public string ComputerName { get; set; }
        public string IPAddress { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<ComputerSystemData> SystemData { get; set; }
        public List<UsageSession> UsageSessions { get; set; }
    }

    public class ActivityReportItem
    {
        public int ComputerID { get; set; }
        public string ComputerName { get; set; }
        public int TotalDurationMinutes { get; set; }
        public int SessionCount { get; set; }
    }
}
