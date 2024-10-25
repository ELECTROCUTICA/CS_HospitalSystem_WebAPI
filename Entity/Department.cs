using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSystem_WebAPI_dotnet6.Entity {

    [Table("Department")]
    public class Department {

        [Key]
        [Column("dep_no")]
        public int Dep_no { get; set; }

        [Column("dep_name")]
        public string? Dep_name { get; set; }

        public Department() { }

        public Department(int dep_no, string dep_name) {
            Dep_no = dep_no;
            Dep_name = dep_name;
        }

    }
}
