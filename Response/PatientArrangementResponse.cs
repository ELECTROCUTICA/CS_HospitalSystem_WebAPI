using HospitalSystem_WebAPI_dotnet6.Entity;

namespace HospitalSystem_WebAPI_dotnet6.Response {

    public class PatientArrangementResponse {

        public Dictionary<int, DateJSON>? Dates { get; set; }

        public string? Now { get; set; }

        public List<Department>? Departments { get; set; }

        public PatientArrangementResponse() { }

        public PatientArrangementResponse(Dictionary<int, DateJSON>? dates, string? now, List<Department>? departments) {
            Dates = dates;
            Now = now;
            Departments = departments;
        }
    }
}
