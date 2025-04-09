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

        public DateTime LastUpdated { get; set; }

        public ICollection<UsageSession> UsageSessions { get; set; } = new List<UsageSession>();

        public ICollection<ComputerSystemData> SystemData { get; set; } = new List<ComputerSystemData>();
        public ComputerSystemData LatestSystemData
        {
            get
            {
                return (SystemData != null && SystemData.Any())
                    ? SystemData.OrderByDescending(s => s.Timestamp).FirstOrDefault()
                    : null;
            }
        }
    }
}
