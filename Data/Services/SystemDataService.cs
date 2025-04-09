using System;
using ComputerTracker.Data.DbModel;

namespace ComputerTracker.Data.Services
{
    public class SystemDataService
    {
        private readonly WMIService _wmiService = new WMIService();

        public void UpdateSystemData(int computerId)
        {
            using (var context = new AppDbContext())
            {
                var computer = context.Computers.Find(computerId);
                if (computer == null)
                    throw new Exception("Компьютер не найден");

                double cpuUsage = _wmiService.GetLocalCPUUsage();
                double memoryUsage = _wmiService.GetLocalMemoryUsage();
                double diskUsage = _wmiService.GetLocalDiskUsage();
                double networkUsage = _wmiService.GetNetworkUsage();
                string osVersion = _wmiService.GetLocalOSVersion();

                var systemData = new ComputerSystemData
                {
                    ComputerID = computer.ComputerID,
                    Timestamp = DateTime.Now,
                    CPUUsage = cpuUsage,
                    MemoryUsage = memoryUsage,
                    DiskUsage = diskUsage,
                    NetworkUsage = networkUsage,
                    OSVersion = osVersion
                };
                context.ComputerSystemDatas.Add(systemData);
                computer.LastUpdated = DateTime.Now;

                context.SaveChanges();
            }
        }
    }
}
