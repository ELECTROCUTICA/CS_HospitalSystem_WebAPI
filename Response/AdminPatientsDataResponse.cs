
using HospitalSystem_WebAPI_dotnet6.Entity;
using HospitalSystem_WebAPI_dotnet6.ViewModels;

namespace HospitalSystem_WebAPI_dotnet6.Response {
    public class AdminPatientsDataResponse {


        public List<PatientView>? Patients { get; set; }

        public int Patients_Count { get; set; }

        public int Pages_Count { get; set; }

        public int Current { get; set; }

        public AdminPatientsDataResponse() { }

        public AdminPatientsDataResponse(List<PatientView> patients, int patients_count, int pages_count, int current) {
            Patients = patients;
            Patients_Count = patients_count;
            Pages_Count = pages_count;
            Current = current;
        }
    }
}
