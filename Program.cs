using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Mapper.Impl;
using SWP391_SE1914_ManageHospital.Service;
using SWP391_SE1914_ManageHospital.Service.Impl;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IClinicMapper, ClinicMapper>();
builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddScoped<IDepartmentMapper, DepartmentMapper>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IpatientMapper, PatientMapper>();
builder.Services.AddScoped<IPatientService, PatientService>();


builder.Services.AddScoped<IPatientFilterMapper, PatientFilterMapper>();
builder.Services.AddScoped<IPatientFilterService, PatientFilterService>();

builder.Services.AddScoped<INurseService, NurseService>();

builder.Services.AddScoped<INurseMapper, NurseMapper>(); 


builder.Services.AddScoped<INurseMapper, NurseMapper>();

builder.Services.AddScoped<IMedicalRecordMapper, MedicalRecordMapper>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<IEmailService, EmailService>();



builder.Services.AddScoped<IMedicineService, MedicineService>();
/*
var hash = BCrypt.Net.BCrypt.HashPassword("Admin1234$");
Console.WriteLine(hash);
*/


var connectionStr = builder.Configuration.GetConnectionString("MySQL");

builder.Services.AddDbContext<ApplicationDBContext>(o =>
    o.UseLazyLoadingProxies()
    .UseMySql(connectionStr, new MySqlServerVersion(new Version(8, 0, 33))));

//chuyen doi datatype enum
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // hoặc chỉ định cụ thể nếu cần: .WithOrigins("http://localhost:5500")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
