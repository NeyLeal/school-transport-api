using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Interfaces;
using SchoolTransport.Infrastructure.Context;
using SchoolTransport.Infrastructure.Repositories;
using SchoolTransport.Application.DTOs.Auth;
using SchoolTransport.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
Console.WriteLine(
    builder.Configuration.GetConnectionString(
        "DefaultConnection"
    )
);
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

builder.Services.AddScoped<IJwtService,JwtService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();