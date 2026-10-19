using Android.App;
using Android.OS;
using Android.Runtime;
using SchoolTransport.Contracts.Dispositivos;

namespace SchoolTransport.App;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override void OnCreate()
    {
        base.OnCreate();
        CriarCanalDeNotificacoes();
    }

    private void CriarCanalDeNotificacoes()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            return;

        var canal = new NotificationChannel(CanaisNotificacao.Viagens, "Viagens", NotificationImportance.High)
        {
            Description = "Embarque, chegada na escola e chegada em casa"
        };

        ((NotificationManager)GetSystemService(NotificationService)!).CreateNotificationChannel(canal);
    }
}
