using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolTransport.Application.Common;
using SchoolTransport.Application.Notificacoes;
using SchoolTransport.Infrastructure.Outbox;
using SchoolTransport.Infrastructure.Persistence;
using SchoolTransport.Infrastructure.Persistence.Repositories;
using SchoolTransport.Infrastructure.Security;
using SchoolTransport.Infrastructure.Servicos;

namespace SchoolTransport.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection não configurada.");

        services.AddDbContext<SchoolTransportDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<SchoolTransportDbContext>());

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IMotoristaRepository, MotoristaRepository>();
        services.AddScoped<IEscolaRepository, EscolaRepository>();
        services.AddScoped<IResponsavelRepository, ResponsavelRepository>();
        services.AddScoped<IAlunoRepository, AlunoRepository>();
        services.AddScoped<IViagemRepository, ViagemRepository>();

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Secao));
        services.AddSingleton<ITokenService, JwtTokenService>();
        services.AddSingleton<ISenhaHasher, BCryptSenhaHasher>();
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<IRelogio, RelogioBrasil>();
        services.AddSingleton<INotificador, LogNotificador>();
        services.AddScoped<OutboxProcessor>();

        return services;
    }
}
