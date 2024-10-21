using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HospitalSystem_WebAPI_dotnet6.ViewModels {

    public class DoctorView {

        public string? ID { get; set; }
        public string? Name { get; set; }
        public string? Sex { get; set; }
        public int Dep_no { get; set; }
        public string? Dep_name { get; set; }
        public string? Title { get; set; }
        public string? Password { get; set; }
        public string? Description { get; set; }

        public DoctorView() { }

        public DoctorView(string id, string name, string sex, int dep_no, string dep_name, string title, string password, string description) {
            ID = id;
            Name = name;
            Sex = sex;
            Dep_no = dep_no;
            Dep_name = dep_name;
            Title = title;
            Password = password;
            Description = description;
        }

    }
}
