using System;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSystem_WebAPI_dotnet6.Entity {

    [Table("Patient")]
    public class Patient {

        [Key]
        [Column("id")]
        public string? ID { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("sex")]
        public string? Sex { get; set; }

        [Column("birthdate")]
        public DateTime Birthdate { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        public Patient() { }

        public Patient(string? id, string? name, string? sex, DateTime birthdate, string? password) {
            ID = id;
            Name = name;
            Sex = sex;
            Birthdate = birthdate;
            Password = password;
        }
    }
}
