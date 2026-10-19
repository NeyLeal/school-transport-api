using Microsoft.JSInterop;
using SchoolTransport.UI.Services;

namespace SchoolTransport.Web.Services;

public class LocalStorageSessao(IJSRuntime js) : ISessaoStorage
{
    public async Task<string?> LerAsync(string chave) =>
        await js.InvokeAsync<string?>("localStorage.getItem", chave);

    public async Task SalvarAsync(string chave, string valor) =>
        await js.InvokeVoidAsync("localStorage.setItem", chave, valor);

    public async Task RemoverAsync(string chave) =>
        await js.InvokeVoidAsync("localStorage.removeItem", chave);
}
