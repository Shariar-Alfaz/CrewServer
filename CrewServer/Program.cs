using System.Globalization;
using System.Text;
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using Services;
using Services.Tools;

var builder = WebApplication.CreateBuilder(args);
var policy = "mypolicy";
builder.Services.AddCors(option=>option.AddPolicy(policy,
    policy =>
    {
        policy.WithOrigins("https://localhost:4200")
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    }
    ));
// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.Siqnature)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );
builder.Services.AddControllers();
builder.Services.AddScoped(typeof(LoginRepo));
builder.Services.AddScoped(typeof(LoginService));
builder.Services.AddScoped(typeof(AdminService));
builder.Services.AddScoped(typeof(AdminRepo));
builder.Services.AddScoped(typeof(AllGuardService));
builder.Services.AddScoped(typeof(AllGuardRepo));
builder.Services.AddScoped(typeof(TokenRepo));
builder.Services.AddScoped(typeof(TeacherRepo));
builder.Services.AddScoped(typeof(TeacherService));
builder.Services.AddScoped(typeof(StudentRepo));
builder.Services.AddScoped(typeof(StudentService));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
