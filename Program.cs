using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Mapper.Impl;
using SWP391_SE1914_ManageHospital.Service;
using SWP391_SE1914_ManageHospital.Service.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IClinicMapper, ClinicMapper>();
builder.Services.AddScoped<IClinicService, ClinicService>();

builder.Services.AddScoped<NurseService>();
builder.Services.AddAutoMapper(typeof(NurseMapper));

var connectionStr = builder.Configuration.GetConnectionString("MySQL");

builder.Services.AddDbContext<ApplicationDBContext>(o =>
    o.UseLazyLoadingProxies()
    .UseMySql(connectionStr, new MySqlServerVersion(new Version(8, 0, 33))));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
