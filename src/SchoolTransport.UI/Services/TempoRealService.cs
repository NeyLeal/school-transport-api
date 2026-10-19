using Microsoft.AspNetCore.SignalR.Client;
using SchoolTransport.Contracts.Viagens;

namespace SchoolTransport.UI.Services;

public sealed class TempoRealService(SessaoService sessao, Uri apiBaseUrl) : IAsyncDisposable
{
    private HubConnection? _conexao;

    public event Action<ViagemResponse>? ViagemAtualizada;

    public async Task ConectarAsync()
    {
        if (_conexao is not null || !sessao.Autenticado)
            return;

        _conexao = new HubConnectionBuilder()
            .WithUrl(new Uri(apiBaseUrl, TempoRealViagens.Rota), opcoes => opcoes.AccessTokenProvider = () => Task.FromResult(sessao.Token))
            .WithAutomaticReconnect()
            .Build();

        _conexao.On<ViagemResponse>(TempoRealViagens.ViagemAtualizada, viagem => ViagemAtualizada?.Invoke(viagem));

        try
        {
            await _conexao.StartAsync();
        }
        catch (Exception)
        {
            // Sem tempo real as telas continuam funcionando pelas requisições normais.
            await DesconectarAsync();
        }
    }

    public async Task DesconectarAsync()
    {
        if (_conexao is null)
            return;

        var conexao = _conexao;
        _conexao = null;
        await conexao.DisposeAsync();
    }

    public ValueTask DisposeAsync() => new(DesconectarAsync());
}
