using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
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
        services.AddScoped<IDispositivoPushRepository, DispositivoPushRepository>();
        services.AddNotificador(configuration);
        services.AddScoped<OutboxProcessor>();

        return services;
    }

    private static void AddNotificador(this IServiceCollection services, IConfiguration configuration)
    {
        var firebase = configuration.GetSection(FirebaseSettings.Secao).Get<FirebaseSettings>() ?? new FirebaseSettings();

        if (string.IsNullOrWhiteSpace(firebase.CredenciaisArquivo))
        {
            services.AddSingleton<INotificador, LogNotificador>();
            return;
        }

        if (!File.Exists(firebase.CredenciaisArquivo))
            throw new InvalidOperationException($"Arquivo de credenciais do Firebase não encontrado: {firebase.CredenciaisArquivo}");

        services.AddSingleton(_ =>
        {
            var credencial = CredentialFactory.FromFile<ServiceAccountCredential>(firebase.CredenciaisArquivo).ToGoogleCredential();
            var app = FirebaseApp.Create(new AppOptions { Credential = credencial }, nameof(SchoolTransport));
            return FirebaseMessaging.GetMessaging(app);
        });
        services.AddScoped<INotificador, FcmNotificador>();
    }
}
