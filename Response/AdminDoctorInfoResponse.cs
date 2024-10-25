
using HospitalSystem_WebAPI_dotnet6.Entity;
using HospitalSystem_WebAPI_dotnet6.ViewModels;

namespace HospitalSystem_WebAPI_dotnet6.Response {
    
    public class AdminDoctorInfoResponse {

        public List<DoctorView>? Doctors { get; set; }

        public List<Department>? Departments { get; set; }

        public int Doctors_Count { get; set; }

        public int Pages_Count { get; set; }

        public int Current { get; set; }

        public AdminDoctorInfoResponse() { }

        public AdminDoctorInfoResponse(List<DoctorView> doctors, List<Department> departments, int doctors_count, int pages_count, int current) { 
            Doctors = doctors;
            Departments = departments;
            Doctors_Count = doctors_count;
            Pages_Count = pages_count;
            Current = current;
        }

    }
}
