using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HospitalSystem_WebAPI_dotnet6.Entity {

    [Table("Doctor")]
    public class Doctor {

        [Key]
        [Column("id")]
        public string? ID { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("sex")]
        public string? Sex { get; set; }

        [Column("dep_no")]
        public int Dep_no { get; set; }

        [Column("title")]
        public string? Title { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Column("description")]
        public string? Description { get; set; }

    }
}
