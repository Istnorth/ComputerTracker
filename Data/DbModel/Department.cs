using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerTracker.Data.DbModel
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string DepartmentName { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }

}
