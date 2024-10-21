using System;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HospitalSystem_WebAPI_dotnet6.Entity {

    [Table("Registration")]
    public class Registration {

        [Key]
        [Column("id")]
        public string? ID { get; set; }

        [Column("doctor_id")]
        public string? Doctor_id { get; set; }

        [Column("patient_id")]
        public string? Patient_id { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("visit_date")]
        public DateTime Visit_date { get; set; }

        public Registration() { }

        public Registration(string id, string doctor_id, string patient_id, int status, DateTime visit_date) {
            ID = id;
            Doctor_id = doctor_id;
            Patient_id = patient_id;
            Status = status;
            Visit_date = visit_date;
        }
    }
}
