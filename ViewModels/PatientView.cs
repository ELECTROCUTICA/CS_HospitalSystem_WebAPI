using HospitalSystem_WebAPI_dotnet6.Entity;

namespace HospitalSystem_WebAPI_dotnet6.ViewModels {


    public class PatientView {

        public string? ID { get; set; }
        public string? Name { get; set; }
        public string? Sex { get; set; }
        public int Age { get; set; }
        public DateTime Birthdate { get; set; }
        public string? Password { get; set; }


        public PatientView() { }

        public PatientView(Patient patient, int age) {
            ID = patient.ID;
            Name = patient.Name;
            Sex = patient.Sex;
            Age = age;
            Birthdate = patient.Birthdate;
            Password = patient.Password;
        }

        public PatientView(string id, string name, string sex, int age, DateTime birthdate, string password) { 
            ID = id;
            Name = name;
            Sex = sex;
            Age = age;
            Birthdate = birthdate; 
            Password = password;
        }
    }
}
