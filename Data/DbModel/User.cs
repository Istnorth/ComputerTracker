using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerTracker.Data.DbModel
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string Fullname { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string Login { get; set; }

        [Column(TypeName = "varchar(120)")]
        public string PasswordHash { get; set; }
        
    }

}
