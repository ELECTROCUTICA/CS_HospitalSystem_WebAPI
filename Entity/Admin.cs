using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSystem_WebAPI_dotnet6.Entity {
   
    [Table("Admin")]
    public class Admin {

        [Key]
        [Column("id")]
        public string? ID { get; set; }


        [Column("password")]
        public string? Password { get; set; }


        public Admin() { }

        public Admin(string id, string password) {
            ID = id;
            Password = password;
        }
    }
}
