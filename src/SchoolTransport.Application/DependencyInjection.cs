using Microsoft.Extensions.DependencyInjection;
using SchoolTransport.Application.Alunos;
using SchoolTransport.Application.Auth;
using SchoolTransport.Application.Escolas;
using SchoolTransport.Application.Motoristas;
using SchoolTransport.Application.Notificacoes;
using SchoolTransport.Application.Responsaveis;
using SchoolTransport.Application.Viagens;

namespace SchoolTransport.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<EscolaService>();
        services.AddScoped<AlunoService>();
        services.AddScoped<ResponsavelService>();
        services.AddScoped<AjudanteService>();
        services.AddScoped<ViagemService>();
        services.AddScoped<NotificacaoViagemService>();

        return services;
    }
}
