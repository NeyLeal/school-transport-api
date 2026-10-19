using SchoolTransport.Application.Common;
using SchoolTransport.Contracts.Escolas;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Application.Escolas;

public class EscolaService(IEscolaRepository escolas, IUsuarioAtual usuarioAtual, IUnitOfWork unitOfWork)
{
    public async Task<IReadOnlyList<EscolaResponse>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var lista = await escolas.ListarAsync(usuarioAtual.MotoristaId, cancellationToken);
        return lista.Select(e => e.ParaResposta()).ToList();
    }

    public async Task<EscolaResponse> ObterAsync(Guid id, CancellationToken cancellationToken = default) =>
        (await ObterEntidadeAsync(id, cancellationToken)).ParaResposta();

    public async Task<EscolaResponse> CriarAsync(EscolaRequest request, CancellationToken cancellationToken = default)
    {
        var escola = new Escola(usuarioAtual.MotoristaId, request.Nome, request.Endereco);
        await escolas.AdicionarAsync(escola, cancellationToken);
        await unitOfWork.SalvarAsync(cancellationToken);
        return escola.ParaResposta();
    }

    public async Task<EscolaResponse> AtualizarAsync(Guid id, EscolaRequest request, CancellationToken cancellationToken = default)
    {
        var escola = await ObterEntidadeAsync(id, cancellationToken);
        escola.Atualizar(request.Nome, request.Endereco);
        await unitOfWork.SalvarAsync(cancellationToken);
        return escola.ParaResposta();
    }

    private async Task<Escola> ObterEntidadeAsync(Guid id, CancellationToken cancellationToken) =>
        await escolas.ObterAsync(usuarioAtual.MotoristaId, id, cancellationToken)
            ?? throw new NaoEncontradoException("Escola não encontrada.");
}
