using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using ComputerTracker.Data.DbModel;
using System.Linq;

namespace ComputerTracker.Data.Services
{
    public class SoftwareUsageMonitor : IDisposable
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private Timer _timer;
        private string _currentProcessName;
        private DateTime _currentStartTime;
        private readonly int _sessionId;
        private bool _isMonitoring;

        public SoftwareUsageMonitor(int sessionId)
        {
            _sessionId = sessionId;
            _currentProcessName = null;
            _isMonitoring = false;
        }
        public void Start()
        {
            if (!_isMonitoring)
            {
                _currentProcessName = GetActiveProcessName();
                _currentStartTime = DateTime.Now;
                _timer = new Timer(MonitorCallback, null, 0, 5000);
                _isMonitoring = true;
            }
        }
        public void Stop()
        {
            if (_isMonitoring)
            {
                RecordUsage(_currentProcessName, _currentStartTime, DateTime.Now);
                _timer?.Dispose();
                _isMonitoring = false;
            }
        }

        private void MonitorCallback(object state)
        {
            string activeProcess = GetActiveProcessName();
            if (activeProcess != _currentProcessName)
            {
                DateTime endTime = DateTime.Now;
                RecordUsage(_currentProcessName, _currentStartTime, endTime);
                _currentProcessName = activeProcess;
                _currentStartTime = endTime;
            }
        }

        private string GetActiveProcessName()
        {
            try
            {
                IntPtr hwnd = GetForegroundWindow();
                if (hwnd == IntPtr.Zero)
                    return null;
                uint processId;
                GetWindowThreadProcessId(hwnd, out processId);
                Process proc = Process.GetProcessById((int)processId);
                return proc.ProcessName;
            }
            catch
            {
                return null;
            }
        }

        private void RecordUsage(string processName, DateTime startTime, DateTime endTime)
        {
            if (string.IsNullOrEmpty(processName))
                return;
            int durationSeconds = (int)(endTime - startTime).TotalSeconds;
            if (durationSeconds < 5)
                return;

            int softwareId = GetOrCreateSoftwareId(processName);

            var usage = new SoftwareUsage
            {
                SessionID = _sessionId,
                SoftwareID = softwareId,
                StartTime = startTime,
                EndTime = endTime,
                Duration = durationSeconds
            };

            using (var context = new AppDbContext())
            {
                context.SoftwareUsages.Add(usage);
                context.SaveChanges();
            }
        }
        private int GetOrCreateSoftwareId(string processName)
        {
            using (var context = new AppDbContext())
            {
                var software = context.Softwares.FirstOrDefault(s => s.SoftwareName.ToLower() == processName.ToLower());
                if (software == null)
                {
                    software = new Software
                    {
                        SoftwareName = processName,
                        Version = "Unknown"
                    };
                    context.Softwares.Add(software);
                    context.SaveChanges();
                }
                return software.SoftwareID;
            }
        }

        public void Dispose()
        {
            Stop();
            _timer?.Dispose();
        }
    }
}
