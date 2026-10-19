namespace SchoolTransport.App;

public static class Configuracao
{
#if DEBUG
    public static Uri ApiBaseUrl => DeviceInfo.Platform == DevicePlatform.Android
        ? new Uri("http://10.0.2.2:5017/")
        : new Uri("https://localhost:7260/");
#else
    public static Uri ApiBaseUrl => new("https://api.schooltransport.com.br/");
#endif
}
