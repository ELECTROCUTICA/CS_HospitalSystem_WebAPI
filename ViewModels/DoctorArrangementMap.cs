
namespace HospitalSystem_WebAPI_dotnet6.ViewModels {

    public class DoctorArrangementMap {
        
        public string? Doctor_id { get; set; }
        public string? Name { get; set; }
        public int Dep_no { get; set; }
        public string? Dep_name { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public int Remain { get; set; }

        public DoctorArrangementMap() { }

        public DoctorArrangementMap(string doctor_id, string name, int dep_no, string dep_name, string title, string description, DateTime date, int remain) {
            Doctor_id = doctor_id;
            Name = name;
            Dep_no = dep_no;
            Dep_name = dep_name;
            Title = title;
            Description = description;
            Date = date;
            Remain = remain;
        }

    }
}
