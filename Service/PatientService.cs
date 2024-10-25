using HospitalSystem_WebAPI_dotnet6.DBConfig;
using HospitalSystem_WebAPI_dotnet6.Entity;
using HospitalSystem_WebAPI_dotnet6.Response;
using HospitalSystem_WebAPI_dotnet6.Utils;
using HospitalSystem_WebAPI_dotnet6.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HospitalSystem_WebAPI_dotnet6.Service {


    public class PatientService : IPatientService {

        private readonly MySQLContext _sqlContext;

        public PatientService() { }

        public PatientService(MySQLContext sqlContext) {
            _sqlContext = sqlContext;
        }

        public async Task<ActionResult<Dictionary<string, object>>> GetServerTime() {
            var task = Task.Run(() => {
                DateTime now = DateTime.Now;
                string week;

                switch (now.DayOfWeek) {
                    case DayOfWeek.Monday:
                        week = "周一";
                        break;
                    case DayOfWeek.Tuesday:
                        week = "周二";
                        break;
                    case DayOfWeek.Wednesday:
                        week = "周三";
                        break;
                    case DayOfWeek.Thursday:
                        week = "周四";
                        break;
                    case DayOfWeek.Friday:
                        week = "周五";
                        break;
                    case DayOfWeek.Saturday:
                        week = "周六";
                        break;
                    case DayOfWeek.Sunday:
                        week = "周日";
                        break;
                    default:
                        week = "异常";
                        throw new Exception("异常");
                }

                string dateStr = $"{now.ToString("D")} {week}";
                string dateParam = now.ToString("yyyy-MM-dd");
                return new Dictionary<string, object> {
                    { "state", "ok"},
                    { "Time", dateStr },
                    { "dateParam", dateParam}
                };

            });

            return await task;
        }


        public async Task<ActionResult<Dictionary<string, object>>> GetRegistrationsToday(string dateParam, PatientView patient) {
            var data = new Dictionary<string, object>();


            if (patient == null) {
                data.Add("state", "fail");
                data.Add("message", "无效的用户登录信息");
            }

            List<RegistrationMap> list = await _sqlContext.RegistrationMaps.Where(v => v.Visit_date.Equals(dateParam) && v.Patient_id == patient.ID && v.Status == 1).ToListAsync();

            if (list == null || list.Count == 0) {
                data.Add("您今日没有预约就诊", -1);
                return data;
            }

            foreach (var item in list) {
                int lineupCount = await _sqlContext.RegistrationMaps.Where(v => v.Visit_date.Equals(dateParam) && v.Doctor_id == item.Doctor_id && v.Status == 1).CountAsync() - 1;

                string message;
                if (lineupCount == 0) {
                    message = $"{item.Dep_name} {item.Doctor_name} {item.Doctor_title}，已排到您，请尽快到{item.Dep_name} {item.Doctor_name}诊室就诊";
                }
                else {
                    message = $"排队中，您还需要等待：{lineupCount} 人";
                }
                data.Add(message, lineupCount);
            }
            return data;
        }


        public async Task<ActionResult<Dictionary<string, object>>> CancelRegistration(string id, PatientView patient) {
            var data = new Dictionary<string, object>();

            if (patient == null) {
                data.Add("state", "fail");
                data.Add("message", "无效的用户登录信息");
            }
            if (string.IsNullOrEmpty(id)) {
                data.Add("state", "fail");
                data.Add("message", "无效的挂号信息");
                return data;
            }

            Registration updated = _sqlContext.Registrations.Find(id);
            updated.Status = 0;
            _sqlContext.Registrations.Update(updated);
            await _sqlContext.SaveChangesAsync();

            data.Add("state", "ok");
            data.Add("message", "取消预约成功");
            return data;

        }


        public async Task<ActionResult<Dictionary<string, object>>> LoginHandle(string id, string password) {
            var data = new Dictionary<string, object>();

            PatientView? patient = _sqlContext.PatientView?.Find(id);

            var task = Task.Run(() => {
                if (patient != null && !string.IsNullOrEmpty(password) && password == _sqlContext.PatientView?.Find(patient.ID)?.Password) {

                    var payload = new Dictionary<string, object> {
                    { "id", patient.ID },
                    { "name", patient.Name },
                    { "sex", patient.Sex },
                    { "age", patient.Age },
                    { "birthdate", patient.Birthdate }
                };

                    var token = JWTUtils.CreateToken(payload);

                    data.Add("state", "ok");
                    data.Add("message", "登陆成功");
                    data.Add("cs_token", token);
                }
                else {
                    data.Add("state", "fail");
                    data.Add("message", "登陆失败，请检查您的身份证号和密码");
                }
                return data;
            });

            return await task;


        }



        public async Task<ActionResult<Dictionary<string, object>>> RegisterHandle(string id, string name, string sex, string birthdate, string password) {
            Patient? patient = _sqlContext.Patients?.Find(id);

            var data = new Dictionary<string, object>();

            if (patient != null) {
                data.Add("state", "duplicated");
                data.Add("message", "注册失败，该身份账号已存在");
                return data;
            }

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(sex) && !string.IsNullOrEmpty(birthdate) && !string.IsNullOrEmpty(password)) {
                DateTime birthdate_obj = DateTime.Parse(birthdate);

                patient = new(id, name, sex, birthdate_obj, password);
                _sqlContext.Patients?.Add(patient);
                await _sqlContext.SaveChangesAsync();
                data.Add("state", "ok");
                data.Add("message", "注册成功，即将返回登录页");
            }
            else {
                data.Add("state", "fail");
                data.Add("message", "注册失败，请检查您的输入是否有误");
            }
            return data;
        }

        public async Task<ActionResult<PatientRecordsResponse>> GetPatientRecords(string p, PatientView patient) {
            int pn = -1;
            if (string.IsNullOrEmpty(p)) pn = 1;
            else pn = int.Parse(p);

            int records_count = 12;
            int total_page_count;

            if (records_count > 0 && records_count % 10 == 0) {
                total_page_count = records_count / 10;
            }
            else {
                total_page_count = records_count / 10 + 1;
            }
            var page = new Page((pn - 1) * 10, 10, pn);


            return new PatientRecordsResponse(await _sqlContext.RegistrationMaps.Where(v => v.Patient_id == patient.ID).OrderByDescending(p => p.ID).Skip((page.Current_Page - 1) * page.Size).Take(page.Size).ToListAsync(),
                records_count,
                total_page_count,
                pn);
        }


        public async Task<ActionResult<Dictionary<string, object>>> EditHandle(string id, string name, string sex, string birthdate, string password, PatientView patient) {
            var data = new Dictionary<string, object>();

            if (patient == null) {
                data.Add("state", "fail");
                data.Add("message", "无效的用户登录信息");
                return data;
            }

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(sex) && !string.IsNullOrEmpty(birthdate) && !string.IsNullOrEmpty(password)) {
                DateTime birthdateobj = DateTime.Parse(birthdate);

                Patient updated = new(patient.ID, name, sex, birthdateobj, password);
                _sqlContext.Patients?.Update(updated);
                await _sqlContext.SaveChangesAsync();

                data.Add("state", "ok");
                data.Add("message", "修改成功，请重新登录");
            }
            else {
                data.Add("state", "fail");
                data.Add("message", "修改失败，请检查您的输入是否有空或有误");
            }
            return data;
        }

        public async Task<ActionResult<PatientArrangementResponse>> GetArrangement() {
            var times = new List<DateTime> {
                DateTime.Now
            };

            var dates = new Dictionary<int, DateJSON>();

            string week;

            string? now = null;
            for (int i = 0; i < 7; i++) {
                times.Add(times[0].AddDays(i + 1));

                switch (times[i].DayOfWeek) {
                    case DayOfWeek.Monday:
                        week = "周一";
                        break;
                    case DayOfWeek.Tuesday:
                        week = "周二";
                        break;
                    case DayOfWeek.Wednesday:
                        week = "周三";
                        break;
                    case DayOfWeek.Thursday:
                        week = "周四";
                        break;
                    case DayOfWeek.Friday:
                        week = "周五";
                        break;
                    case DayOfWeek.Saturday:
                        week = "周六";
                        break;
                    case DayOfWeek.Sunday:
                        week = "周日";
                        break;
                    default:
                        week = "异常";
                        throw new Exception("异常");
                }
                if (i == 0) {
                    now = $"{ times[i].ToString("yyyy年MM月dd日") } {week}";
                }
                dates.Add(i, new DateJSON($"{ times[i].ToString("yyyy年MM月dd日") } {week}", $"{ times[i].ToString("yyyy-MM-dd") }"));

            }
            List<Department>? departments = await _sqlContext.Departments!.ToListAsync();
            return new PatientArrangementResponse(dates, now, departments);
        }

        public async Task<ActionResult<List<DoctorArrangementMap>>> GetDoctorsWorkAtDate(int dep_no, string date) =>
            await _sqlContext.DoctorArrangementsMap!.Where(v => v.Dep_no == dep_no && v.Date.Equals(Convert.ToDateTime(date))).ToListAsync();



        public async Task<ActionResult<Dictionary<string, object>>> GetDoctorDescription(string doctor_id, string date) {
            var data = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(doctor_id) || string.IsNullOrEmpty(date)) {
                data.Add("state", "fail");
                data.Add("message", "提交参数异常");
            }
            else {
                data.Add("state", "ok");
                data.Add("description", _sqlContext.DoctorView.Where(v => v.ID == doctor_id).Select(o => o.Description));
                data.Add("message", "请求成功");
            }
            return data;
        }


        public ActionResult<Dictionary<string, object>> RegistrationSubmit(string doctor_id, string date, PatientView patient) {
            var data = new Dictionary<string, object>();


            if (patient != null && !string.IsNullOrEmpty(doctor_id) && !string.IsNullOrEmpty(date)) {
                List<RegistrationMap> collections = _sqlContext.RegistrationMaps.Where(v => v.Patient_id == patient.ID && v.Visit_date.Equals(date)).ToList();

                foreach (var registration in collections) {
                    if (doctor_id == registration.Doctor_id && registration.Status == 1) {
                        data.Add("state", "duplicated");
                        data.Add("message", "预约失败，请不要重复预约");
                        return data;
                    }
                }

                DoctorArrangement target = _sqlContext.DoctorArrangements.Where(v => v.Doctor_id == doctor_id && v.Date.Equals(Convert.ToDateTime(date))).First();
                if (target.Remain > 0) {

                    //事务处理
                    using var transcation = _sqlContext.Database.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        target.Remain -= 1;
                        _sqlContext.DoctorArrangements.Update(target);
                        string lastID = _sqlContext.Registrations.OrderByDescending(p => p.ID).First().ID;

                        Registration new_registration = new(Convert.ToString(Convert.ToInt64(lastID) + 1), doctor_id, patient.ID, 1, DateTime.Parse(date));

                        _sqlContext.Registrations.Add(new_registration);
                        _sqlContext.SaveChanges();
                        transcation.Commit();

                        data.Add("state", "ok");
                        data.Add("message", "预约成功");
                    }
                    catch (Exception ex) {
                        Console.WriteLine(ex.StackTrace);
                        transcation.RollbackAsync();
                        data.Add("state", "fail");
                        data.Add("message", "执行异常，事务被回滚");
                    }
                    return data;
                }
                else {
                    data.Add("state", "no_remain");
                    data.Add("message", "预约失败，该医师已无剩余号源");
                    return data;
                }

            }
            else {
                data.Add("state", "fail");
                data.Add("message", "无效的预约信息/身份信息");
                return data;
            }
        }

        public async Task<ActionResult<int>> GetCounts() =>
            await _sqlContext.Patients!.CountAsync();


        public ActionResult<string> SayHello() => "Hello from .Net Core 6.0 WebApi";
    }
}
