using Android.App;
using AndroidX.Core.App;
using Firebase.Messaging;
using SchoolTransport.Contracts.Dispositivos;

namespace SchoolTransport.App.Services;

/// <summary>
/// Com o app em segundo plano o Android exibe o push sozinho; em primeiro plano ele chega aqui e é exibido manualmente.
/// </summary>
[Service(Exported = false)]
[IntentFilter(["com.google.firebase.MESSAGING_EVENT"])]
public class ViagensMessagingService : FirebaseMessagingService
{
    public override void OnMessageReceived(RemoteMessage message)
    {
        if (message.GetNotification() is not { } notificacao)
            return;

        var intent = PackageManager?.GetLaunchIntentForPackage(PackageName!);
        var abrirApp = intent is null
            ? null
            : PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.Immutable | PendingIntentFlags.UpdateCurrent);

        var builder = new NotificationCompat.Builder(this, CanaisNotificacao.Viagens);
        builder.SetSmallIcon(Resource.Mipmap.appicon);
        builder.SetContentTitle(notificacao.Title);
        builder.SetContentText(notificacao.Body);
        builder.SetStyle(new NotificationCompat.BigTextStyle().BigText(notificacao.Body));
        builder.SetPriority(NotificationCompat.PriorityHigh);
        builder.SetAutoCancel(true);

        if (abrirApp is not null)
            builder.SetContentIntent(abrirApp);

        NotificationManagerCompat.From(this)!.Notify(message.MessageId?.GetHashCode() ?? Environment.TickCount, builder.Build()!);
    }
}
