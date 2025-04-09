using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerTracker.Data.DbModel
{
    public class Software
    {
        [Key]
        public int SoftwareID { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string SoftwareName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Version { get; set; }

        public ICollection<SoftwareUsage> SoftwareUsages { get; set; } = new List<SoftwareUsage>();
    }

}
