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

namespace HospitalSystem_WebAPI_dotnet6.Service {

    public class AdminService : IAdminService {

        private readonly MySQLContext _sqlContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminService() { }

        public AdminService(MySQLContext sqlContext, IHttpContextAccessor httpContextAccessor) {
            _sqlContext = sqlContext ?? throw new ArgumentNullException(nameof(sqlContext));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<ActionResult<Dictionary<string, object>>> LoginHandle(string id, string password) {
            Admin admin = _sqlContext.Admins?.Find(id);

            var task = Task.Run(() => {
                var data = new Dictionary<string, object>();
                if (admin != null && password == admin.Password) {
                    _httpContextAccessor.HttpContext?.Session.SetString("Admin", admin.ID);              //Session.Set()方法需要对实例对象进行序列化
                    data.Add("state", "ok");
                    data.Add("message", "登陆成功");
                }
                else {
                    data.Add("state", "fail");
                    data.Add("message", "登陆失败");
                }
                return data;
            });
            return await task;
        }

        public async Task<ActionResult<AdminDoctorInfoResponse>> DoctorinfoInterface(string? p, string? keyword) {
            int pn;

            if (string.IsNullOrEmpty(p)) pn = 1;
            else pn = int.Parse(p);

            int doctors_count = _sqlContext.DoctorView?.Count() != null ? await _sqlContext.DoctorView.CountAsync() : 0;
            int total_page_count;
            List<DoctorView> doctors;
            if (!string.IsNullOrEmpty(keyword)) {

                if (doctors_count > 0 && doctors_count % 10 == 0) {
                    total_page_count = doctors_count / 10;
                }
                else {
                    total_page_count = doctors_count / 10 + 1;
                }
                Page page = new Page((pn - 1) * 10, 10, pn);
                doctors = await _sqlContext.DoctorView.Where(v => EF.Functions.Like($"%{keyword}", v.ID) || EF.Functions.Like($"%{keyword}", v.Name)).Skip((page.Current_Page - 1) * page.Size).Take(page.Size).ToListAsync();
            }
            else {
                if (doctors_count > 0 && doctors_count % 10 == 0) {
                    total_page_count = doctors_count / 10;
                }
                else {
                    total_page_count = doctors_count / 10 + 1;
                }
                Page page = new Page((pn - 1) * 10, 10, pn);
                doctors = await _sqlContext.DoctorView.Skip((page.Current_Page - 1) * page.Size).Take(page.Size).ToListAsync();
            }

            List<Department> departments = await _sqlContext.Departments.ToListAsync();

            return new AdminDoctorInfoResponse(doctors, departments, doctors_count, total_page_count, pn);
        }


        public async Task<ActionResult<Dictionary<string, object>>> DeleteDoctor(string id) {
            var doctor = _sqlContext.Doctors.Find(id);

            var task = Task.Run(() => {
                var data = new Dictionary<string, object>();
                if (doctor != null) {
                    _sqlContext.Doctors.Remove(doctor);
                    _sqlContext.SaveChanges();
                    data.Add("state", "ok");
                    data.Add("message", "删除医生信息成功");
                }
                else {
                    data.Add("state", "fail");
                    data.Add("message", "删除失败");
                }
                return data;
            });

            return await task;

        }

        public async Task<ActionResult<Dictionary<string, object>>> UpdateDoctor(string id, string name, string sex, int dep_no, string title, string password, string description) {
            var data = new Dictionary<string, object>();
            var doctor = _sqlContext.Doctors.Find(id);

            if (doctor != null) {
                using var transaction = _sqlContext.Database.BeginTransaction(IsolationLevel.ReadCommitted);
                try {
                    doctor.Name = name;
                    doctor.Sex = sex;
                    doctor.Dep_no = dep_no;
                    doctor.Title = title;
                    doctor.Password = password;
                    doctor.Description = description;
                    _sqlContext.Doctors.Update(doctor);
                    await _sqlContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    data.Add("state", "ok");
                    data.Add("message", "修改完成");
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.StackTrace);
                    await transaction.RollbackAsync();
                    data.Add("state", "fail");
                    data.Add("message", "修改失败");
                }
            }
            else {
                data.Add("state", "fail");
                data.Add("message", "修改失败");
            }
            return data;
        }

        public async Task<ActionResult<Dictionary<string, object>>> InsertDoctor(string id, string name, string sex, int dep_no, string title, string password, string description) {
            var data = new Dictionary<string, object>();
            Doctor? doctor = _sqlContext.Doctors.Find(id);


            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(sex) && dep_no > 0 && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(description)) {
                if (_sqlContext.Doctors.Contains(doctor)) {
                    data.Add("state", "fail");
                    data.Add("message", "新建失败，请查看是否已存在该医生");
                }
                else {
                    Doctor new_doctor = new(id, name, sex, dep_no, title, password, description);
                    await _sqlContext.Doctors.AddAsync(new_doctor);
                    await _sqlContext.SaveChangesAsync();
                    data.Add("state", "ok");
                    data.Add("message", "创建医生信息完成");
                }
            }
            else {
                data.Add("state", "fail");
                data.Add("message", "新建失败，请查看输入信息是否有效");
            }
            return data;
        }

        public async Task<ActionResult<Department>> GetDepartment(int dep_no) => await _sqlContext.Departments.FindAsync(dep_no);

        public async Task<ActionResult<List<Department>>> GetDepartments() => await _sqlContext.Departments.ToListAsync();

        public async Task<ActionResult<List<DoctorView>>> GetDoctors(int dep_no) => await _sqlContext.DoctorView.Where(v => v.Dep_no == dep_no).ToListAsync();


        public async Task<ActionResult<Dictionary<string, object>>> InsertDepartment(int dep_no, string dep_name) {
            var data = new Dictionary<string, object>();
            Department? department = _sqlContext.Departments.Find(dep_no);

            if (!_sqlContext.Departments.Contains(department) || !string.IsNullOrEmpty(dep_name)) {
                if (dep_no < 0) {
                    data.Add("state", "fail");
                    data.Add("message", "非法的科室编号");
                    return data;
                }
                Department new_department = new(dep_no, dep_name);
                await _sqlContext.Departments.AddAsync(new_department);
                data.Add("state", "ok");
                data.Add("message", "创建科室成功");
            }
            else {
                data.Add("state", "fail");
                data.Add("message", "创建失败，请检查是否存在重复的科室");
            }
            return data;

        }

        public async Task<ActionResult<Dictionary<string, object>>> UpdateDepartment(int dep_no, string dep_name) {
            var data = new Dictionary<string, object>();

            if (dep_no > 0 && !string.IsNullOrEmpty(dep_name)) {
                Department department = _sqlContext.Departments.Find(dep_no);
                department.Dep_name = dep_name;
                _sqlContext.Departments.Update(department);
                await _sqlContext.SaveChangesAsync();
                data.Add("state", "ok");
                data.Add("message", "修改成功");
            }
            else {
                data.Add("state", "fail");
                data.Add("message", "修改失败");
            }
            return data;
        }

        public async Task<ActionResult<Dictionary<string, object>>> Transfer(int source, int target) {
            var data = new Dictionary<string, object>();

            if (source >= 0 && target >= 0) {
                var doctors = _sqlContext.Doctors.Where(doctor => doctor.Dep_no == source).ToArray();
                foreach (var doctor in doctors) {
                    doctor.Dep_no = target;
                }

                _sqlContext.Doctors.UpdateRange(doctors);
                await _sqlContext.SaveChangesAsync();
                data.Add("state", "ok");
                data.Add("message", "迁移完成");
            }
            else {
                data.Add("state", "fail");
                data.Add("message", "迁移失败");
            }
            return data;
        }

        public async Task<ActionResult<AdminArrangementResponse>> GetSchedule() {
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
            return new AdminArrangementResponse(dates, now!, departments);
        }


        public async Task<ActionResult<List<DoctorView>>> GetDoctorsNoWorkAtDate(int dep_no, string date) {
            return await _sqlContext.DoctorView.FromSqlRaw($"select * from DoctorView where dep_no = {dep_no} and id not in ( select doctor_id from DoctorArrangement where date = '{date}')").ToListAsync();

            //沟槽的linq lambda表达式
            //var doctors = await _sqlContext.DoctorView.Where(doctor => 
            //doctor.Dep_no == dep_no && 
            //!_sqlContext.DoctorArrangements.Where(v => v.Date.Equals(Convert.ToDateTime(date))).Select(r => r.Doctor_id).Contains(doctor.ID)).ToListAsync();
            //return doctors;
        }

        public async Task<ActionResult<List<DoctorView>>> GetDoctorsWorkAtDate(int dep_no, string date) =>
            await _sqlContext.DoctorView.FromSqlRaw($"select * from DoctorView where dep_no = {dep_no} and id in ( select doctor_id from DoctorArrangement where date = '{date}')").ToListAsync();

        public async Task<ActionResult<Dictionary<string, object>>> CancelSchedule(string date, string doctor_id) {
            throw new NotImplementedException();
        }

        public async Task<ActionResult<DoctorView>> GetDoctor(string id) {
            throw new NotImplementedException();
        }

        public async Task<ActionResult<Dictionary<string, object>>> GoToWork(string date, string doctor_id, int remain) {
            throw new NotImplementedException();
        }


        public async Task<ActionResult<AdminPatientsDataResponse>> PatientManager(string p, string keyword) {
            throw new NotImplementedException();
        }

        public async Task<ActionResult<Dictionary<string, object>>> ResetPassword(string p_id) {
            throw new NotImplementedException();
        }



    }
}
