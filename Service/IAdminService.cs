using HospitalSystem_WebAPI_dotnet6.Entity;
using HospitalSystem_WebAPI_dotnet6.Response;
using HospitalSystem_WebAPI_dotnet6.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem_WebAPI_dotnet6.Service {


    public interface IAdminService {

        public Task<ActionResult<Dictionary<string, object>>> LoginHandle(string id, string password);

        public Task<ActionResult<AdminDoctorInfoResponse>> DoctorinfoInterface(string? p, string? keyword);

        public Task<ActionResult<Dictionary<string, object>>> DeleteDoctor(string id);

        public Task<ActionResult<DoctorView>> GetDoctor(string id);

        public Task<ActionResult<Dictionary<string, object>>> UpdateDoctor(string id, string name, string sex, int dep_no, string title, string password, string description);

        public Task<ActionResult<Dictionary<string, object>>> InsertDoctor(string id, string name, string sex, int dep_no, string title, string password, string description);

        public Task<ActionResult<List<Department>>> GetDepartments();

        public Task<ActionResult<Department>> GetDepartment(int dep_no);

        public Task<ActionResult<List<DoctorView>>> GetDoctors(int dep_no);

        public Task<ActionResult<Dictionary<string, object>>> InsertDepartment(int dep_no, string dep_name);

        public Task<ActionResult<Dictionary<string, object>>> UpdateDepartment(int dep_no, string dep_name);

        public Task<ActionResult<Dictionary<string, object>>> Transfer(int source, int target);

        public Task<ActionResult<AdminArrangementResponse>> GetSchedule();

        public Task<ActionResult<List<DoctorView>>> GetDoctorsNoWorkAtDate(int dep_no, string date);

        public Task<ActionResult<List<DoctorView>>> GetDoctorsWorkAtDate(int dep_no, string date);

        public Task<ActionResult<Dictionary<string, object>>> GoToWork(string date, string doctor_id, int remain);

        public Task<ActionResult<Dictionary<string, object>>> CancelSchedule(string date, string doctor_id);


        public Task<ActionResult<AdminPatientsDataResponse>> PatientManager(string p, string keyword);


        public Task<ActionResult<Dictionary<string, object>>> ResetPassword(string p_id);
    }
}
