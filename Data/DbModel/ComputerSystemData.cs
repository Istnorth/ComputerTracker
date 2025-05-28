using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerTracker.Data.DbModel
{
    public class ComputerSystemData
    {
        [Key]
        public int SystemDataID { get; set; }

        [Required]
        public int ComputerID { get; set; }

        [ForeignKey(nameof(ComputerID))]
        public Computer Computer { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // ——— Поля OS ———
        [Column(TypeName = "varchar(150)")]
        public string OSVersion { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string OSCaption { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string OSManufacturer { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string WindowsDirectory { get; set; }

        // ——— Поля CPU ———
        [Column(TypeName = "varchar(200)")]
        public string CPUName { get; set; }
        public int CpuCores { get; set; }
        public int CpuThreads { get; set; }
        public int CpuClockMHz { get; set; }

        // ——— Навесные коллекции устройств и логов ———
        public ICollection<ComputerGpu> Gpus { get; set; } = new List<ComputerGpu>();
        public ICollection<Keyboard> Keyboards { get; set; } = new List<Keyboard>();
        public ICollection<Mouse> Mice { get; set; } = new List<Mouse>();
        public ICollection<Printer> Printers { get; set; } = new List<Printer>();
        public ICollection<Scanner> Scanners { get; set; } = new List<Scanner>();
        public ICollection<Monitor> Monitors { get; set; } = new List<Monitor>();
        public ICollection<KeyLogEntry> KeyLog { get; set; } = new List<KeyLogEntry>();
        public ICollection<AppUsageEntry> AppUsage { get; set; } = new List<AppUsageEntry>();
    }
}
