using Android.Gms.Extensions;
using Firebase.Messaging;
using SchoolTransport.UI.Services;

namespace SchoolTransport.App.Services;

public class FirebasePushProvider : IPushProvider
{
    public string Plataforma => "android";

    public async Task<string?> ObterTokenAsync()
    {
        // Android 13+ exige permissão explícita para exibir notificações.
        var status = await MainThread.InvokeOnMainThreadAsync(Permissions.RequestAsync<Permissions.PostNotifications>);

        if (status != PermissionStatus.Granted)
            return null;

        // Obsoleto pela migração do Firebase para IDs de instalação; o servidor ainda envia por token (ver FcmNotificador).
#pragma warning disable CS0618
        var token = await FirebaseMessaging.Instance.GetToken().AsAsync<Java.Lang.String>();
#pragma warning restore CS0618
        return token?.ToString();
    }
}
