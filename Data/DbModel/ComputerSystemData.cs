using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerTracker.Data.DbModel
{
    public class ComputerSystemData
    {
        [Key]
        public int SystemDataID { get; set; }

        public int ComputerID { get; set; }
        public Computer Computer { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public double CPUUsage { get; set; }

        public double MemoryUsage { get; set; }

        public double DiskUsage { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string OSVersion { get; set; }

        public double NetworkUsage { get; set; }
    }


}
