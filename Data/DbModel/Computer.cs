using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ComputerTracker.Data.DbModel
{
    public class Computer
    {
        [Key]
        public int ComputerID { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ComputerName { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string IPAddress { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Host { get; set; }
        public int Port { get; set; }

        public DateTime LastUpdated { get; set; }

        public ICollection<UsageSession> UsageSessions { get; set; } = new List<UsageSession>();
        public ICollection<ComputerSystemData> SystemData { get; set; } = new List<ComputerSystemData>();
        public ICollection<ComputerGpu> Gpus { get; set; } = new List<ComputerGpu>();
        public ICollection<Keyboard> Keyboards { get; set; } = new List<Keyboard>();
        public ICollection<Mouse> Mice { get; set; } = new List<Mouse>();
        public ICollection<Printer> Printers { get; set; } = new List<Printer>();
        public ICollection<Scanner> Scanners { get; set; } = new List<Scanner>();
        public ICollection<Monitor> Monitors { get; set; } = new List<Monitor>();
        public ICollection<KeyLogEntry> KeyLogEntries { get; set; } = new List<KeyLogEntry>();
        public ICollection<AppUsageEntry> AppUsageEntries { get; set; } = new List<AppUsageEntry>();

        [NotMapped]
        public ComputerSystemData LatestSystemData =>
            SystemData != null && SystemData.Any()
                ? SystemData.OrderByDescending(s => s.Timestamp).First()
                : null;
    }
}
