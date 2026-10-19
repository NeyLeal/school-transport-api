using System.Text.Json;
using SchoolTransport.Contracts.Auth;
using SchoolTransport.Contracts.Comum;

namespace SchoolTransport.UI.Services;

public class SessaoService(ISessaoStorage storage)
{
    private const string Chave = "schooltransport.sessao";
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    private LoginResponse? _sessao;
    private bool _carregada;

    public event Action? Alterada;

    public bool Autenticado => _sessao is not null && _sessao.ExpiraEm > DateTime.UtcNow;
    public string? Token => Autenticado ? _sessao!.Token : null;
    public UsuarioResponse? Usuario => Autenticado ? _sessao!.Usuario : null;
    public bool EhMotorista => Usuario?.Perfil == Perfil.Motorista;
    public bool EhResponsavel => Usuario?.Perfil == Perfil.Responsavel;

    public async Task CarregarAsync()
    {
        if (_carregada)
            return;

        var json = await storage.LerAsync(Chave);

        if (!string.IsNullOrWhiteSpace(json))
        {
            try
            {
                _sessao = JsonSerializer.Deserialize<LoginResponse>(json, Json);
            }
            catch (JsonException)
            {
                await storage.RemoverAsync(Chave);
            }
        }

        _carregada = true;
    }

    public async Task EntrarAsync(LoginResponse sessao)
    {
        _sessao = sessao;
        _carregada = true;
        await storage.SalvarAsync(Chave, JsonSerializer.Serialize(sessao, Json));
        Alterada?.Invoke();
    }

    public async Task SairAsync()
    {
        _sessao = null;
        await storage.RemoverAsync(Chave);
        Alterada?.Invoke();
    }
}
