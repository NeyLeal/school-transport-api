using Microsoft.Extensions.DependencyInjection;

namespace SchoolTransport.UI.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSchoolTransportUi(this IServiceCollection services, Uri apiBaseUrl)
    {
        services.AddScoped<SessaoService>();
        services.AddHttpClient<ApiClient>(client => client.BaseAddress = apiBaseUrl);
        return services;
    }
}
