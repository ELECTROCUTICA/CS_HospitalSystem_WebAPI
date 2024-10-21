
using HospitalSystem_WebAPI_dotnet6.Entity;
using HospitalSystem_WebAPI_dotnet6.Response;
using HospitalSystem_WebAPI_dotnet6.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem_WebAPI_dotnet6.Service {

    public interface IPatientService {

        public Task<ActionResult<Dictionary<string, object>>> GetServerTime();

        public Task<ActionResult<Dictionary<string, object>>> GetRegistrationsToday(string dateParam, PatientView patient);

        public Task<ActionResult<Dictionary<string, object>>> LoginHandle(string id, string password);

        public Task<ActionResult<Dictionary<string, object>>> RegisterHandle(string id, string name, string sex, string birthdate, string password);

        public Task<ActionResult<PatientRecordsResponse>> GetPatientRecords(string p, PatientView patient);

        public Task<ActionResult<Dictionary<string, object>>> CancelRegistration(string id, PatientView patient);

        public Task<ActionResult<Dictionary<string, object>>> EditHandle(string id, string name, string sex, string birthdate, string password, PatientView patient);

        public Task<ActionResult<PatientArrangementResponse>> GetArrangement();

        public Task<ActionResult<List<DoctorArrangementMap>>> GetDoctorsWorkAtDate(int dep_no, string date);

        public Task<ActionResult<Dictionary<string, object>>> GetDoctorDescription(string doctor_id, string date);

        public Task<ActionResult<Dictionary<string, object>>> RegistrationSubmit(string doctor_id, string date, PatientView patient);

        public Task<ActionResult<int>> GetCounts();

        public ActionResult<string> SayHello();
    }
}
