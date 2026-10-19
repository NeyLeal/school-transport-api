namespace SchoolTransport.UI.Services;

public interface ISessaoStorage
{
    Task<string?> LerAsync(string chave);
    Task SalvarAsync(string chave, string valor);
    Task RemoverAsync(string chave);
}
