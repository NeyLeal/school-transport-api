using SchoolTransport.Contracts.Viagens;

namespace SchoolTransport.Application.Viagens;

public interface IPublicadorViagens
{
    Task ViagemAtualizadaAsync(Guid motoristaId, ViagemResponse viagem, CancellationToken cancellationToken = default);
}
