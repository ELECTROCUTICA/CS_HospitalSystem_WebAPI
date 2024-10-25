
using HospitalSystem_WebAPI_dotnet6.Entity;

namespace HospitalSystem_WebAPI_dotnet6.Response {
    public class AdminPatientsDataResponse {


        public List<Patient>? Patients { get; set; }

        public int Patients_Count { get; set; }

        public int Pages_Count { get; set; }

        public int Current { get; set; }

        public AdminPatientsDataResponse() { }

        public AdminPatientsDataResponse(List<Patient> patients, int patients_count, int pages_count, int current) {
            Patients = patients;
            Patients_Count = patients_count;
            Pages_Count = pages_count;
            Current = current;
        }
    }
}
