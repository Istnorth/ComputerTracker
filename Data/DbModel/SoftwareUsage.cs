using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerTracker.Data.DbModel
{
    public class SoftwareUsage
    {
        [Key]
        public int SoftwareUsageID { get; set; }

        public int SessionID { get; set; }
        public UsageSession Session { get; set; }

        public int SoftwareID { get; set; }
        public Software Software { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Duration { get; set; }
    }

}
