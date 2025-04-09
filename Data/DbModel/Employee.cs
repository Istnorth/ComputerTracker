using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerTracker.Data.DbModel
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string MiddleName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }

        public int DepartmentID { get; set; }
        public Department Department { get; set; }
        public string FullName => $"{LastName} {FirstName} {MiddleName}".Replace("  ", " ").Trim();

        public ICollection<UsageSession> UsageSessions { get; set; } = new List<UsageSession>();
    }

}
