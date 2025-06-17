using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Mapper.Impl;
using SWP391_SE1914_ManageHospital.Service;
using SWP391_SE1914_ManageHospital.Service.Impl;
using SWP391_SE1914_ManageHospital.Ultility;
using System.Text;
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
builder.Services.AddScoped<IRoleMapper, RoleMapper>();
builder.Services.AddScoped<IRoleService, RoleService>();



builder.Services.AddScoped<IPatientFilterMapper, PatientFilterMapper>();
builder.Services.AddScoped<IPatientFilterService, PatientFilterService>();




builder.Services.AddScoped<INurseService, NurseService>();
builder.Services.AddScoped<INurseMapper, NurseMapper>();

builder.Services.AddScoped<IMedicalRecordMapper, MedicalRecordMapper>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();


builder.Services.AddScoped<INurseService, NurseService>();
builder.Services.AddScoped<INurseMapper, NurseMapper>();


builder.Services.AddScoped<IMedicineService, MedicineService>();

builder.Services.AddScoped<IMedicineCategoryService, MedicineCategoryService>();
builder.Services.AddScoped<IMedicineCategoryMapper, MedicineCategoryMapper>();


builder.Services.AddScoped<INurseService, NurseService>();
builder.Services.AddScoped<INurseMapper, NurseMapper>(); 



var connectionStr = builder.Configuration.GetConnectionString("MySQL");

builder.Services.AddDbContext<ApplicationDBContext>(o =>
    o.UseLazyLoadingProxies()
    .UseMySql(connectionStr, new MySqlServerVersion(new Version(8, 0, 33))));


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

//  CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .AllowAnyOrigin()     //  origin
                .AllowAnyMethod()      //  HTTP methods
                .AllowAnyHeader();     //  headers
        });
});
//Quazt
builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("MyCronJob");

    q.AddJob<MyCronJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("MyCronTrigger")
        .WithSimpleSchedule(x => x.WithIntervalInSeconds(60).RepeatForever()));
});
// Đăng ký Hosted Service cho Quartz
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    // Thêm logging để debug
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine("OnChallenge: " + context.Error);
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebBanAoo API", Version = "v1" });

    // Th�m c?u h�nh security
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
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

app.UseAuthorization();

app.MapControllers();

app.Run();
