using SchoolTransport.UI.Services;

namespace SchoolTransport.App.Services;

public class SecureStorageSessao : ISessaoStorage
{
    public Task<string?> LerAsync(string chave) => SecureStorage.Default.GetAsync(chave);

    public Task SalvarAsync(string chave, string valor) => SecureStorage.Default.SetAsync(chave, valor);

    public Task RemoverAsync(string chave)
    {
        SecureStorage.Default.Remove(chave);
        return Task.CompletedTask;
    }
}
