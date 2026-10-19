using SchoolTransport.API.Extensions;
using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Interfaces;
using SchoolTransport.Infrastructure.Context;
using SchoolTransport.Infrastructure.Repositories;
using SchoolTransport.Application.DTOs.Auth;
using SchoolTransport.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApiComJwt();
builder.Services.AddDbContext<SchoolTransportDbContext>(
    options =>
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "DefaultConnection"));
    });
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(
        "Jwt"));
var jwtSettings =
    builder.Configuration
        .GetSection("Jwt")
        .Get<JwtSettings>();

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,

                ValidIssuer = jwtSettings!.Issuer,
                ValidAudience = jwtSettings.Audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings.Key))
            };
    });

builder.Services.AddScoped<IJwtService,JwtService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISchoolRepository, SchoolRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IGuardianRepository, GuardianRepository>();
builder.Services.AddScoped<IStudentGuardianRepository, StudentGuardianRpository>();

builder.Services.AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseOpenApiComSwaggerUi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();