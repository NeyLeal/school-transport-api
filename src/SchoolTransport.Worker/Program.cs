using SchoolTransport.Application.Notificacoes;
using SchoolTransport.Infrastructure;
using SchoolTransport.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<NotificacaoViagemService>();
builder.Services.AddHostedService<OutboxWorker>();

var host = builder.Build();
host.Run();
