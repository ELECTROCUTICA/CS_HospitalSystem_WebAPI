
namespace HospitalSystem_WebAPI_dotnet6.ViewModels {

    public class RegistrationMap {

        public string? ID { get; set; }
        public string? Doctor_id { get; set; }
        public string? Doctor_name { get; set; }
        public string? Doctor_title { get; set; }
        public int Dep_no { get; set; }
        public string? Dep_name { get; set; }
        public string? Patient_id { get; set; }
        public string? Patient_name { get; set; }
        public string? Patient_sex { get; set; }
        public int Patient_age { get; set; }
        public DateTime Patient_birthdate { get; set; }
        public int Status { get; set; }
        public string? Visit_date { get; set; }


        public RegistrationMap() { }

        public RegistrationMap(string id, string doctor_id, string doctor_name, string doctor_title, int dep_no, string dep_name, string patient_id, string patient_name, string patient_sex, int patient_age,
            DateTime patient_birthdate, int status, string visit_date) {
            ID = id;
            Doctor_id = doctor_id;
            Doctor_name = doctor_name;
            Doctor_title = doctor_title;
            Dep_no = dep_no;
            Dep_name = dep_name;
            Patient_id = patient_id;
            Patient_name = patient_name;
            Patient_sex = patient_sex;
            Patient_age = patient_age;
            Patient_birthdate = patient_birthdate;
            Status = status;
            Visit_date = visit_date;
        }
    }
}
