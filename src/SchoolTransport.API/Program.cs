using Microsoft.EntityFrameworkCore;
using SchoolTransport.API.Extensions;
using SchoolTransport.API.Hubs;
using SchoolTransport.API.Services;
using SchoolTransport.Application;
using SchoolTransport.Application.Common;
using SchoolTransport.Application.Viagens;
using SchoolTransport.Contracts.Viagens;
using SchoolTransport.Infrastructure;
using SchoolTransport.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUsuarioAtual, UsuarioAtualHttp>();
builder.Services.AddAutenticacaoJwt(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<TratadorDeExcecoes>();
builder.Services.AddOpenApiComJwt();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddScoped<IPublicadorViagens, SignalRPublicadorViagens>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApiComSwaggerUi();

    if (app.Configuration.GetValue<bool>("Database:AplicarMigracoesAoIniciar"))
    {
        using var scope = app.Services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<SchoolTransportDbContext>().Database.MigrateAsync();
    }
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ViagensHub>($"/{TempoRealViagens.Rota}");

app.Run();
