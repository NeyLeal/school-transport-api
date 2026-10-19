using SchoolTransport.Application.Common;
using SchoolTransport.Contracts.Ajudantes;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Application.Motoristas;

public class AjudanteService(IMotoristaRepository motoristas, IUsuarioAtual usuarioAtual, IUnitOfWork unitOfWork)
{
    public async Task<IReadOnlyList<AjudanteResponse>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var motorista = await ObterMotoristaAsync(cancellationToken);
        return motorista.Ajudantes.OrderBy(a => a.Nome).Select(a => a.ParaResposta()).ToList();
    }

    public async Task<AjudanteResponse> AdicionarAsync(AjudanteRequest request, CancellationToken cancellationToken = default)
    {
        var motorista = await ObterMotoristaAsync(cancellationToken);
        var ajudante = motorista.AdicionarAjudante(request.Nome, request.Telefone);
        await unitOfWork.SalvarAsync(cancellationToken);
        return ajudante.ParaResposta();
    }

    public async Task RemoverAsync(Guid ajudanteId, CancellationToken cancellationToken = default)
    {
        var motorista = await ObterMotoristaAsync(cancellationToken);
        motorista.RemoverAjudante(ajudanteId);
        await unitOfWork.SalvarAsync(cancellationToken);
    }

    private async Task<Motorista> ObterMotoristaAsync(CancellationToken cancellationToken) =>
        await motoristas.ObterPorIdAsync(usuarioAtual.MotoristaId, cancellationToken)
            ?? throw new NaoEncontradoException("Motorista não encontrado.");
}
