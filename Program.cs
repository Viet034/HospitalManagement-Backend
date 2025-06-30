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
using SWP391_SE1914_ManageHospital.Service;
using SWP391_SE1914_ManageHospital.Ultility;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddHttpContextAccessor();

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
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleMapper, RoleMapper>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<IDoctorMapper, DoctorMapper>();
builder.Services.AddScoped<IDoctorService, DoctorService>();

builder.Services.AddScoped<IEmailService, EmailService>();



builder.Services.AddScoped<IPatientFilterMapper, PatientFilterMapper>();
builder.Services.AddScoped<IPatientFilterService, PatientFilterService>();
builder.Services.AddScoped<INurseMapper, NurseMapper>();
builder.Services.AddScoped<INurseService, NurseService>();

builder.Services.AddScoped<IMedicineService, MedicineService>();
builder.Services.AddScoped<IMedicineMapper, MedicineMapper>();

builder.Services.AddScoped<IMedicineCategoryService, MedicineCategoryService>();
builder.Services.AddScoped<IMedicineCategoryMapper, MedicineCategoryMapper>();

// Đăng ký service
builder.Services.AddScoped<IMedicineCategoryService, MedicineCategoryService>();



builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddScoped<ISupplierMapper, SupplierMapper>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IMedicineImportMapper, MedicineImportMapper>();
builder.Services.AddScoped<IMedicineImportService, MedicineImportService>();
builder.Services.AddScoped<IMedicineImportDetailMapper, MedicineImportDetailMapper>();
builder.Services.AddScoped<IMedicineImportDetailService, MedicineImportDetailService>();

builder.Services.AddScoped<IMedicineManageForAdminService, MedicineManageForAdminService>();
builder.Services.AddScoped<IMedicineManageForAdminMapper, MedicineManageForAdminMapper>();





builder.Services.AddScoped<INurseService, NurseService>();
builder.Services.AddScoped<INurseMapper, NurseMapper>();

builder.Services.AddScoped<IMedicalRecordListMapper, MedicalRecordListMapper>();
builder.Services.AddScoped<IMedicalRecordListService, MedicalRecordListService>();

builder.Services.AddScoped<IMedicalRecordDetailMapper, MedicalRecordDetailMapper>();
builder.Services.AddScoped<IMedicalRecordDetailService, MedicalRecordDetailService>();






var hash = BCrypt.Net.BCrypt.HashPassword("Admin1234$");
Console.WriteLine(hash);






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
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
//QuaztMore actions
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

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();


