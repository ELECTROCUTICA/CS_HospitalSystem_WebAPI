using MySql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HospitalSystem_WebAPI_dotnet6.Entity;
using HospitalSystem_WebAPI_dotnet6.ViewModels;
using MySql.Data;
using MySql.EntityFrameworkCore.Query;
using HospitalSystem_WebAPI_dotnet6.Utils;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HospitalSystem_WebAPI_dotnet6.DBConfig {

    public class MySQLContext : DbContext {

        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

        public MySQLContext() { }

        /// <summary>
        /// 基本表
        /// </summary>

        public DbSet<Admin>? Admins { get; set; }

        public DbSet<Department>? Departments { get; set; }

        public DbSet<Patient>? Patients { get; set; }

        public DbSet<Doctor>? Doctors { get; set; }

        public DbSet<DoctorArrangement>? DoctorArrangements { get; set; }

        public DbSet<Registration>? Registrations { get; set; }


        /// <summary>
        /// 视图
        /// </summary>

        public DbSet<RegistrationMap>? RegistrationMaps { get; set; }

        public DbSet<DoctorArrangementMap>? DoctorArrangementsMap { get; set; }

        public DbSet<DoctorView>? DoctorView { get; set; }

        public DbSet<PatientView>? PatientView { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<RegistrationMap>().ToView("RegistrationView").Property(e => e.Visit_date).HasConversion(
                v => Convert.ToDateTime(v),
                v => v.ToString("yyyy-MM-dd"));

            modelBuilder.Entity<PatientView>().ToView("PatientView").HasKey(e => e.ID);

            modelBuilder.Entity<DoctorArrangementMap>().ToView("ArrangementView").HasNoKey();

            modelBuilder.Entity<DoctorView>().ToView("DoctorView").HasKey(e => e.ID);
        }


    }
}
