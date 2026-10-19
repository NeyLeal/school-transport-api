using Microsoft.Extensions.Logging;
using SchoolTransport.App.Services;
using SchoolTransport.UI.Services;

namespace SchoolTransport.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddSingleton<ISessaoStorage, SecureStorageSessao>();
#if ANDROID
        builder.Services.AddSingleton<IPushProvider, FirebasePushProvider>();
#endif
        builder.Services.AddSchoolTransportUi(Configuracao.ApiBaseUrl);

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
