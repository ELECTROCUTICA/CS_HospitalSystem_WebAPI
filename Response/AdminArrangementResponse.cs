using HospitalSystem_WebAPI_dotnet6.Entity;
using HospitalSystem_WebAPI_dotnet6.Utils;

namespace HospitalSystem_WebAPI_dotnet6.Response {
    public class AdminArrangementResponse {

        public Dictionary<int, DateJSON>? Dates { get; set; }

        public string? Now { get; set; }

        public List<Department>? Departments { get; set; }

        public AdminArrangementResponse() { }

        public AdminArrangementResponse(Dictionary<int, DateJSON> dates, string now, List<Department> departments) { 
            Dates = dates;
            Now = now;
            Departments = departments;
        }
    }
}
