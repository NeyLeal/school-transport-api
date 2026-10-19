using Microsoft.AspNetCore.SignalR;
using SchoolTransport.API.Hubs;
using SchoolTransport.Application.Viagens;
using SchoolTransport.Contracts.Viagens;

namespace SchoolTransport.API.Services;

public class SignalRPublicadorViagens(IHubContext<ViagensHub> hub, ILogger<SignalRPublicadorViagens> logger) : IPublicadorViagens
{
    public async Task ViagemAtualizadaAsync(Guid motoristaId, ViagemResponse viagem, CancellationToken cancellationToken = default)
    {
        try
        {
            await hub.Clients
                .Groups(ViagensHub.GrupoMotorista(motoristaId), ViagensHub.GrupoAluno(viagem.AlunoId))
                .SendAsync(TempoRealViagens.ViagemAtualizada, viagem, cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            // A viagem já foi salva; tempo real é melhor esforço e não deve falhar a requisição.
            logger.LogWarning(ex, "Falha ao publicar a atualização da viagem {ViagemId}", viagem.Id);
        }
    }
}
