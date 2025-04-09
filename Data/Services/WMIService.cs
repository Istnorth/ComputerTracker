using System;
using System.Management;

namespace ComputerTracker.Data.Services
{
    public class WMIService
    {
        public double GetLocalCPUUsage()
        {
            double cpuUsage = 0.0;
            try
            {
                string query = "SELECT LoadPercentage FROM Win32_Processor";
                var searcher = new ManagementObjectSearcher("root\\cimv2", query);
                foreach (ManagementObject obj in searcher.Get())
                {
                    cpuUsage = Convert.ToDouble(obj["LoadPercentage"]);
                    break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении загрузки CPU: " + ex.Message);
            }
            return cpuUsage;
        }

        public double GetLocalMemoryUsage()
        {
            double memoryUsage = 0.0;
            try
            {
                var searcher = new ManagementObjectSearcher("root\\cimv2", "SELECT FreePhysicalMemory, TotalVisibleMemorySize FROM Win32_OperatingSystem");
                foreach (ManagementObject obj in searcher.Get())
                {
                    double free = Convert.ToDouble(obj["FreePhysicalMemory"]);
                    double total = Convert.ToDouble(obj["TotalVisibleMemorySize"]);
                    memoryUsage = 100 - (free / total * 100);
                    break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении использования памяти: " + ex.Message);
            }
            return memoryUsage;
        }
        public double GetLocalDiskUsage()
        {
            double diskUsage = 0.0;
            try
            {
                var searcher = new ManagementObjectSearcher("root\\cimv2", "SELECT FreeSpace, Size FROM Win32_LogicalDisk WHERE DeviceID='C:'");
                foreach (ManagementObject obj in searcher.Get())
                {
                    double free = Convert.ToDouble(obj["FreeSpace"]);
                    double total = Convert.ToDouble(obj["Size"]);
                    diskUsage = 100 - (free / total * 100);
                    break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении использования диска: " + ex.Message);
            }
            return diskUsage;
        }

        public double GetNetworkUsage()
        {
            double totalMbps = 0.0;

            try
            {
                var searcher = new ManagementObjectSearcher(
                    "root\\CIMV2",
                    "SELECT Name, BytesTotalPerSec FROM Win32_PerfFormattedData_Tcpip_NetworkInterface"
                );

                foreach (ManagementObject obj in searcher.Get())
                {
                    double bytesPerSec = Convert.ToDouble(obj["BytesTotalPerSec"]);

                    double bitsPerSec = bytesPerSec * 8;
                    double mbps = bitsPerSec / (1024 * 1024);

                    totalMbps += mbps;
                }
            }
            catch (Exception ex)
            {
                return 0.0;
            }

            return totalMbps;
        }

        public string GetLocalOSVersion()
        {
            string osVersion = string.Empty;
            try
            {
                var searcher = new ManagementObjectSearcher("root\\cimv2", "SELECT Caption FROM Win32_OperatingSystem");
                foreach (ManagementObject obj in searcher.Get())
                {
                    osVersion = obj["Caption"]?.ToString();
                    break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении версии ОС: " + ex.Message);
            }
            return osVersion;
        }
    }
}
