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

        var token = await FirebaseMessaging.Instance.GetToken().AsAsync<Java.Lang.String>();
        return token?.ToString();
    }
}
