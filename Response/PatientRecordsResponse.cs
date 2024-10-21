using HospitalSystem_WebAPI_dotnet6.ViewModels;

namespace HospitalSystem_WebAPI_dotnet6.Response {

    public class PatientRecordsResponse {

        public List<RegistrationMap>? Registrations { get; set; }

        public int Records_Count { get; set; }

        public int Pages_Count { get; set; }

        public int Current { get; set; }

        public PatientRecordsResponse() { }

        public PatientRecordsResponse(List<RegistrationMap> registrations, int records_count, int pages_count, int current) {
            Registrations = registrations;
            Records_Count = records_count;
            Pages_Count = pages_count;
            Current = current;
        }

    }
}
