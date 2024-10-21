using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalSystem_WebAPI_dotnet6.DBConfig;
using HospitalSystem_WebAPI_dotnet6.Entity;
using HospitalSystem_WebAPI_dotnet6.ViewModels;
using System.Text;
using System.Text.Json;
using JWT;
using MySql.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Query;
using HospitalSystem_WebAPI_dotnet6.Utils;
using HospitalSystem_WebAPI_dotnet6.Response;
using HospitalSystem_WebAPI_dotnet6.Service;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace HospitalSystem_WebAPI_dotnet6.Controllers {

    [Route("patient/api/")]
    [ApiController]
    public class PatientController : ControllerBase {

        private readonly IPatientService _patientService;

        private readonly IDistributedCache _distributedCache; 
        
        private readonly IConnectionMultiplexer _redis;

        public PatientController(IPatientService patientService, IDistributedCache distributedCache) {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
            _redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");

        }

        [HttpGet]
        [Route("getPatient")]
        public ActionResult<PatientView> GetPatient() {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            return JWTUtils.GetPatientFromToken<PatientView>(token);
        }

        [HttpGet]
        [Route("getServerTime")]
        public async Task<ActionResult<Dictionary<string, object>>> GetSeverTime() {
            return await _patientService.GetServerTime();
        }

        [HttpGet]
        [Route("getRegistrationsToday")]
        public async Task<ActionResult<Dictionary<string, object>>> GetRegistrationsToday(string dateParam) {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            PatientView patient = JWTUtils.GetPatientFromToken<PatientView>(token);

            return await _patientService.GetRegistrationsToday(dateParam, patient);
        }

        [HttpGet]
        [Route("logout")]
        public async Task<ActionResult<Dictionary<string, object>>> Logout() {
            return new Dictionary<string, object> {
                { "state", "ok"},
                { "message", "登出成功"}
            };
        }


        [HttpPost]
        [Route("loginHandle")]                        //在.net core开发中，post请求需要在形参前标注[FromForm]
        public async Task<ActionResult<Dictionary<string, object>>> LoginHandle([FromForm] string id, [FromForm] string password) => await _patientService.LoginHandle(id, password);


        [HttpPost]
        [Route("register/registerHandle")]
        public async Task<ActionResult<Dictionary<string, object>>> RegisterHandle([FromForm] string id, [FromForm] string name, [FromForm] string sex, [FromForm] string birthdate, [FromForm] string password) {
            return await _patientService.RegisterHandle(id, name, sex, birthdate, password);
        }



        [HttpGet]
        [Route("getRecords")]
        public async Task<ActionResult<PatientRecordsResponse>> GetPatientRecords(string p) {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            PatientView patient = JWTUtils.GetPatientFromToken<PatientView>(token);

            byte[]? result = _distributedCache.Get($"{patient.ID}_{p}");
            if (result != null) {
                PatientRecordsResponse? data = JsonSerializer.Deserialize<PatientRecordsResponse>(Encoding.UTF8.GetString(result))!;
                return data;
            }
            else {
                var opt = new DistributedCacheEntryOptions {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(90)
                };

                PatientRecordsResponse? data = _patientService.GetPatientRecords(p, patient).Result.Value;
                string jsonString = JsonSerializer.Serialize(data);

                await _distributedCache.SetAsync($"{patient.ID}_{p}", Encoding.UTF8.GetBytes(jsonString), opt);
                return await _patientService.GetPatientRecords(p, patient);
            }

        }

        [HttpPost]
        [Route("cancelRegistration")]
        public async Task<ActionResult<Dictionary<string, object>>> CancelRegistration([FromForm] string id) {

            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            PatientView patient = JWTUtils.GetPatientFromToken<PatientView>(token);

            var response = await RedisUtils.PatientRecordsRemover(_redis, patient);

            return await _patientService.CancelRegistration(id, patient);
        }


        [HttpPost]
        [Route("edit/editHandle")]
        public async Task<ActionResult<Dictionary<string, object>>> EditHandle([FromForm] string id, [FromForm] string name, [FromForm] string sex, [FromForm] string birthdate, [FromForm] string password) {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            return await _patientService.EditHandle(id, name, sex, birthdate, password, JWTUtils.GetPatientFromToken<PatientView>(token));
        }


        [HttpGet]
        [Route("getArrangement")]
        public async Task<ActionResult<PatientArrangementResponse>> GetArrangement() {
            return await _patientService.GetArrangement();
        }

        [HttpGet]
        [Route("registration/getDoctorsWorkAtDate")]
        public async Task<ActionResult<List<DoctorArrangementMap>>> GetDoctorsWorkAtDate(int dep_no, string date) => 
            await _patientService.GetDoctorsWorkAtDate(dep_no, date);

        [HttpGet]
        [Route("registration/getDescription")]
        public async Task<ActionResult<Dictionary<string, object>>> GetDescription(string doctor_id, string date) =>
            await _patientService.GetDoctorDescription(doctor_id, date);

        [HttpPost]
        [Route("registration/submit")]
        public async Task<ActionResult<Dictionary<string, object>>> RegistrationSubmit([FromForm] string date, [FromForm] string doctor_id) {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            PatientView patient = JWTUtils.GetPatientFromToken<PatientView>(token);

            var response = await RedisUtils.PatientRecordsRemover(_redis, patient);

            return await _patientService.RegistrationSubmit(date, doctor_id, patient);
        }

        [HttpPost]
        [Route("requestAI")]
        public async Task<ActionResult<string>> SendRequestToChatGPT([FromForm] string message) => await ChatGPTAPI.SendRequestToChatGPT(message);


        [HttpGet]
        [Route("hello")]
        public ActionResult<string> SayHello() => _patientService.SayHello();


    }
}
