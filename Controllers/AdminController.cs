using HospitalSystem_WebAPI_dotnet6.Entity;
using HospitalSystem_WebAPI_dotnet6.Response;
using HospitalSystem_WebAPI_dotnet6.ViewModels;
using HospitalSystem_WebAPI_dotnet6.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace HospitalSystem_WebAPI_dotnet6.Controllers {


    [Route("admin/api/")]
    [ApiController]
    public class AdminController : ControllerBase {

        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService) {
            _adminService = adminService ?? throw new ArgumentNullException(nameof(adminService));
        }

        [HttpPost]
        [Route("loginHandle")]
        public async Task<ActionResult<Dictionary<string, object>>> LoginHandle([FromForm] string id, [FromForm] string password) =>
            await _adminService.LoginHandle(id, password);

        [HttpGet]
        [Route("getAdmin")]
        public async Task<ActionResult<string?>> GetAdmin() => HttpContext.Session.GetString("Admin");

        [HttpGet]
        [Route("logout")]
        public async Task<ActionResult<Dictionary<string, object>>> Logout() {
            HttpContext.Session.Remove("Admin");
            return new Dictionary<string, object> {
                { "state", "ok" },
                { "message", "登出成功" }
            };
        }

        [HttpGet]
        [Route("doctorinfo")]
        public async Task<ActionResult<AdminDoctorInfoResponse>> DoctorInfoInterface(string? p, string? keyword) {

            return await _adminService.DoctorinfoInterface(p, keyword);
        }

        [HttpPost]
        [Route("doctorinfo/delete")]
        public async Task<ActionResult<Dictionary<string, object>>> DeleteDoctor([FromForm] string id) =>
            await _adminService.DeleteDoctor(id);

        [HttpGet]
        [Route("doctorinfo/getDoctor")]
        public async Task<ActionResult<DoctorView>> GetDoctor(string id) =>
             await _adminService.GetDoctor(id);


        [HttpPost]
        [Route("doctorinfo/update")]
        public async Task<ActionResult<Dictionary<string, object>>> UpdateDoctor([FromForm] string? id, [FromForm] string? name, [FromForm] string? sex, [FromForm] int dep_no, [FromForm] string? title,
            [FromForm] string? password, [FromForm] string? description) =>
            await _adminService.UpdateDoctor(id, name, sex, dep_no, title, password, description);


        [HttpPost]
        [Route("doctorinfo/insert")]
        public async Task<ActionResult<Dictionary<string, object>>> InsertDoctor([FromForm] string? id, [FromForm] string? name, [FromForm] string? sex, [FromForm] int dep_no, [FromForm] string? title,
            [FromForm] string? password, [FromForm] string? description) =>
            await _adminService.InsertDoctor(id, name, sex, dep_no, title, password, description);

        [HttpGet]
        [Route("getDepartments")]
        public async Task<ActionResult<List<Department>>> GetDepartments() => await _adminService.GetDepartments();

        [HttpGet]
        [Route("getDepartment")]
        public async Task<ActionResult<Department>> GetDepartment(int dep_no) => await _adminService.GetDepartment(dep_no);

        [HttpGet]
        [Route("department/getDoctors")]
        public async Task<ActionResult<List<DoctorView>>> GetDoctors(int dep_no) => await _adminService.GetDoctors(dep_no);

        [HttpPost]
        [Route("department/insert")]
        public async Task<ActionResult<Dictionary<string, object>>> InsertDepartment([FromForm] int dep_no, [FromForm] string dep_name) => await _adminService.InsertDepartment(dep_no, dep_name);

        [HttpPost]
        [Route("department/update")]
        public async Task<ActionResult<Dictionary<string, object>>> UpdateDepartment([FromForm] int dep_no, [FromForm] string dep_name) => await _adminService.UpdateDepartment(dep_no, dep_name);

        [HttpPost]
        [Route("department/transfer")]
        public async Task<ActionResult<Dictionary<string, object>>> Transfer([FromForm] int source, [FromForm] int target) => await _adminService.Transfer(source, target);

        [HttpGet]
        [Route("getSchedule")]
        public async Task<ActionResult<AdminArrangementResponse>> GetSchedule() => await _adminService.GetSchedule();

        [HttpGet]
        [Route("schedule/getDoctorsNoWorkAtDate")]
        public async Task<ActionResult<List<DoctorView>>> GetDoctorsNoWorkAtDate(int dep_no, string date) => await _adminService.GetDoctorsNoWorkAtDate(dep_no, date);

        [HttpGet]
        [Route("schedule/getDoctorsWorkAtDate")]
        public async Task<ActionResult<List<DoctorView>>> GetDoctorsWorkAtDate(int dep_no, string date) => await _adminService.GetDoctorsWorkAtDate(dep_no, date);




    }
}
