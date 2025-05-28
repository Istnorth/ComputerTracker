using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerTracker.Data.DbModel
{
    public class ComputerGpu
    {
        [Key]
        public int ComputerGpuID { get; set; }

        [Required]
        public int ComputerId { get; set; }

        [ForeignKey(nameof(ComputerId))]
        public Computer Computer { get; set; }

        [Required, Column(TypeName = "varchar(200)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string DriverVersion { get; set; }

        public long AdapterRAM { get; set; }
    }
}
