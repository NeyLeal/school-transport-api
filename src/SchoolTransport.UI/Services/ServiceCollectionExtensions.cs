using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SchoolTransport.UI.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSchoolTransportUi(this IServiceCollection services, Uri apiBaseUrl)
    {
        services.AddScoped<SessaoService>();
        services.AddHttpClient<ApiClient>(client => client.BaseAddress = apiBaseUrl);
        services.AddScoped(sp => new TempoRealService(sp.GetRequiredService<SessaoService>(), apiBaseUrl));
        services.TryAddSingleton<IPushProvider, SemPush>();
        services.AddScoped<NotificacoesPush>();
        return services;
    }
}
