using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using ComputerTracker.Data.DbModel;
using Microsoft.EntityFrameworkCore;
using Monitor = ComputerTracker.Data.DbModel.Monitor;

namespace ComputerTracker.Data.Services
{
    public class SystemDataService
    {
        private readonly HttpClient _http = new HttpClient();

        public void UpdateSystemData(int computerId)
        {
            using var context = new AppDbContext();
            var computer = context.Computers
                                  .Include(c => c.SystemData)
                                  .Include(c => c.Gpus)
                                  .Include(c => c.Keyboards)
                                  .Include(c => c.Mice)
                                  .Include(c => c.Printers)
                                  .Include(c => c.Scanners)
                                  .Include(c => c.Monitors)
                                  .Include(c => c.KeyLogEntries)
                                  .Include(c => c.AppUsageEntries)
                                  .FirstOrDefault(c => c.ComputerID == computerId);

            if (computer == null)
                throw new Exception("Компьютер не найден");

            var url = $"http://{computer.Host}:{computer.Port}/api/systeminfo";
            var json = _http.GetStringAsync(url).GetAwaiter().GetResult();

            // 2) Десериализуем в DTO
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var dto = JsonSerializer.Deserialize<SystemInfoDto>(json, options);
            if (dto == null)
                throw new Exception("Не удалось распарсить JSON");

            // 3) Создаем новую запись ComputerSystemData
            var sys = new ComputerSystemData
            {
                ComputerID = computer.ComputerID,
                Timestamp = DateTime.UtcNow,
                OSCaption = dto.Os.Caption,
                OSVersion = dto.Os.Version,
                OSManufacturer = dto.Os.Manufacturer,
                WindowsDirectory = dto.Os.WindowsDirectory,
                CPUName = dto.Cpu.Name,
                CpuCores = int.Parse(dto.Cpu.Cores),
                CpuThreads = int.Parse(dto.Cpu.Threads),
                CpuClockMHz = int.Parse(dto.Cpu.ClockMHz)
            };
            context.ComputerSystemDatas.Add(sys);

            // 4) GPU
            foreach (var g in dto.Gpu ?? new List<GpuDto>())
            {
                context.Gpus.Add(new ComputerGpu
                {
                    ComputerId = computer.ComputerID,
                    Name = g.Name,
                    DriverVersion = g.DriverVersion,
                    AdapterRAM = long.Parse(g.AdapterRAM)
                });
            }

            // 5) Keyboards
            if (dto.Keyboards != null)
                foreach (var k in dto.Keyboards)
                    context.Keyboards.Add(new Keyboard
                    {
                        ComputerId = computer.ComputerID,
                        Name = k.Name,
                        Description = k.Description,
                        DeviceID = k.DeviceID,
                        Manufacturer = k.Manufacturer,
                        Status = k.Status
                    });

            // 6) Mice
            if (dto.Mice != null)
                foreach (var m in dto.Mice)
                    context.Mice.Add(new Mouse
                    {
                        ComputerId = computer.ComputerID,
                        Name = m.Name,
                        Description = m.Description,
                        DeviceID = m.DeviceID,
                        Manufacturer = m.Manufacturer,
                        Status = m.Status
                    });

            // 7) Printers
            if (dto.Printers != null)
                foreach (var p in dto.Printers)
                    context.Printers.Add(new Printer
                    {
                        ComputerId = computer.ComputerID,
                        Name = p.Name,
                        Status = p.Status
                    });

            // 8) Scanners
            if (dto.Scanners != null)
                foreach (var s in dto.Scanners)
                    context.Scanners.Add(new Scanner
                    {
                        ComputerId = computer.ComputerID,
                        Name = s.Name,
                        Status = s.Status
                    });

            // 9) Monitors
            if (dto.Monitors != null)
                foreach (var mon in dto.Monitors)
                    context.Monitors.Add(new Monitor
                    {
                        ComputerId = computer.ComputerID,
                        Name = mon.Name,
                        Manufacturer = mon.Manufacturer,
                        Status = mon.Status
                    });

            // 10) KeyLog
            if (dto.KeyLog != null)
                foreach (var k in dto.KeyLog)
                    context.KeyLogEntries.Add(new KeyLogEntry
                    {
                        ComputerId = computer.ComputerID,
                        Key = k.Key,
                        Time = DateTime.Parse(k.Time)
                    });

            // 11) AppUsage
            if (dto.AppUsage != null)
                foreach (var a in dto.AppUsage)
                    context.AppUsageEntries.Add(new AppUsageEntry
                    {
                        ComputerId = computer.ComputerID,
                        WindowTitle = a.WindowTitle,
                        Duration = TimeSpan.Parse(a.Duration)
                    });

            computer.LastUpdated = DateTime.UtcNow;
            context.SaveChanges();
        }

        // DTO-классы для десериализации
        private class SystemInfoDto
        {
            public OsDto Os { get; set; }
            public CpuDto Cpu { get; set; }
            public List<GpuDto> Gpu { get; set; }
            public List<DeviceDto> Keyboards { get; set; }
            public List<DeviceDto> Mice { get; set; }
            public List<DeviceDto> Printers { get; set; }
            public List<DeviceDto> Scanners { get; set; }
            public List<DeviceDto> Monitors { get; set; }
            public List<KeyLogDto> KeyLog { get; set; }
            public List<AppUsageDto> AppUsage { get; set; }
        }
        private class OsDto
        {
            public string ComputerName { get; set; }
            public string Caption { get; set; }
            public string Version { get; set; }
            public string Manufacturer { get; set; }
            public string WindowsDirectory { get; set; }
        }
        private class CpuDto
        {
            public string Name { get; set; }
            public string Cores { get; set; }
            public string Threads { get; set; }
            public string ClockMHz { get; set; }
        }
        private class GpuDto
        {
            public string Name { get; set; }
            public string DriverVersion { get; set; }
            public string AdapterRAM { get; set; }
        }
        private class DeviceDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string DeviceID { get; set; }
            public string Manufacturer { get; set; }
            public string Status { get; set; }
        }
        private class KeyLogDto
        {
            public string Key { get; set; }
            public string Time { get; set; }
        }
        private class AppUsageDto
        {
            public string WindowTitle { get; set; }
            public string Duration { get; set; }
        }
    }
}
