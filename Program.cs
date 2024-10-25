using HospitalSystem_WebAPI_dotnet6.DBConfig;
using HospitalSystem_WebAPI_dotnet6.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


//跨域
builder.Services.AddCors(options => {
    options.AddPolicy("CorsPolicy1", policy => {
        policy
        .WithOrigins("http://localhost:8081")
        .AllowAnyHeader()
        .WithMethods(HttpMethods.Options, HttpMethods.Get, HttpMethods.Post, HttpMethods.Put, HttpMethods.Delete)
        .AllowCredentials()
        .SetPreflightMaxAge(TimeSpan.FromHours(24));
    });
});

//redis
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "127.0.0.1:6379,allowadmin=true";
    options.InstanceName = "redis_";
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();


//mysql连接
string connectString = "Server=localhost;Port=3306;Database=HospitalDB;Uid=root;Pwd=991234;";
builder.Services.AddDbContext<MySQLContext>(options => options.UseMySQL(connectString));

//Service层
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAdminService, AdminService>();


var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseAuthorization();
app.UseRouting();

app.UseCors("CorsPolicy1");

app.UseSession();

app.MapControllers();

app.Run();
