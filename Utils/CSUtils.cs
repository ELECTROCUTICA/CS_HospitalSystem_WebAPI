using System;
using System.Text;
using System.Linq;
using HospitalSystem_WebAPI_dotnet6.ViewModels;
using HospitalSystem_WebAPI_dotnet6.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSystem_WebAPI_dotnet6.DBConfig;

namespace HospitalSystem_WebAPI_dotnet6.Utils {


    public class CSUtils {

        public static Func<string, Patient> GetPatientFunc() => (token) => JWTUtils.GetPatientFromToken<Patient>(token);
        public static Func<string, PatientView> GetPatientViewFunc() => (token) => JWTUtils.GetPatientFromToken<PatientView>(token);

    }
}
