using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerTracker.Data.DbModel
{
    public class UsageSession
    {
        [Key]
        public int SessionID { get; set; }

        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        public int ComputerID { get; set; }
        public Computer Computer { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Duration { get; set; }

        public ICollection<SoftwareUsage> SoftwareUsages { get; set; } = new List<SoftwareUsage>();
    }

}
