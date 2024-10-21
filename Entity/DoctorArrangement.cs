using System;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HospitalSystem_WebAPI_dotnet6.Entity {

    [Table("DoctorArrangement")]
    public class DoctorArrangement {

        [Key]
        [Column("date")]
        public DateTime Date { get; set; }

        [Column("doctor_id")]
        public string? Doctor_id { get; set; }

        [Column("remain")]
        public int Remain { get; set; }
    }
}
