using SchoolTransport.Contracts.Dispositivos;

namespace SchoolTransport.UI.Services;

/// <summary>Fonte do token de push da plataforma. O app Android implementa com Firebase; a web não recebe push.</summary>
public interface IPushProvider
{
    string Plataforma { get; }
    Task<string?> ObterTokenAsync();
}

public sealed class SemPush : IPushProvider
{
    public string Plataforma => "web";
    public Task<string?> ObterTokenAsync() => Task.FromResult<string?>(null);
}

public class NotificacoesPush(ApiClient api, IPushProvider push)
{
    private string? _tokenRegistrado;

    public async Task RegistrarAsync()
    {
        try
        {
            var token = await push.ObterTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
                return;

            await api.RegistrarDispositivoAsync(new RegistrarDispositivoRequest { Token = token, Plataforma = push.Plataforma });
            _tokenRegistrado = token;
        }
        catch (Exception)
        {
            // Push é complementar: sem permissão, sem Google Play ou offline, o app segue normalmente.
        }
    }

    public async Task RemoverAsync()
    {
        if (_tokenRegistrado is not { } token)
            return;

        _tokenRegistrado = null;

        try
        {
            await api.RemoverDispositivoAsync(new RemoverDispositivoRequest { Token = token });
        }
        catch (ApiException)
        {
        }
    }
}
