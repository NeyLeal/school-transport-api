using SchoolTransport.Application.Common;
using SchoolTransport.Contracts.Alunos;
using SchoolTransport.Domain.Entities;
using SchoolTransport.Domain.Enums;

namespace SchoolTransport.Application.Alunos;

public class AlunoService(
    IAlunoRepository alunos,
    IEscolaRepository escolas,
    IResponsavelRepository responsaveis,
    IUsuarioAtual usuarioAtual,
    IUnitOfWork unitOfWork)
{
    public async Task<IReadOnlyList<AlunoResponse>> ListarAsync(bool incluirInativos, CancellationToken cancellationToken = default)
    {
        var lista = await alunos.ListarAsync(usuarioAtual.MotoristaId, incluirInativos, cancellationToken);
        return lista.Select(a => a.ParaResposta()).ToList();
    }

    public async Task<AlunoDetalheResponse> ObterAsync(Guid id, CancellationToken cancellationToken = default) =>
        Detalhar(await ObterEntidadeAsync(id, cancellationToken));

    public async Task<AlunoResponse> CriarAsync(AlunoRequest request, CancellationToken cancellationToken = default)
    {
        await GarantirEscolaAsync(request.EscolaId, cancellationToken);

        var aluno = new Aluno(
            usuarioAtual.MotoristaId,
            request.EscolaId,
            request.Nome,
            request.DataNascimento,
            request.ValorMensalidade,
            request.EnderecoPadraoIda,
            request.EnderecoPadraoVolta);

        await alunos.AdicionarAsync(aluno, cancellationToken);
        await unitOfWork.SalvarAsync(cancellationToken);

        return (await ObterEntidadeAsync(aluno.Id, cancellationToken)).ParaResposta();
    }

    public async Task<AlunoResponse> AtualizarAsync(Guid id, AlunoRequest request, CancellationToken cancellationToken = default)
    {
        var aluno = await ObterEntidadeAsync(id, cancellationToken);
        await GarantirEscolaAsync(request.EscolaId, cancellationToken);

        aluno.Atualizar(
            request.EscolaId,
            request.Nome,
            request.DataNascimento,
            request.ValorMensalidade,
            request.EnderecoPadraoIda,
            request.EnderecoPadraoVolta);

        await unitOfWork.SalvarAsync(cancellationToken);
        return (await ObterEntidadeAsync(id, cancellationToken)).ParaResposta();
    }

    public async Task DefinirAtivoAsync(Guid id, bool ativo, CancellationToken cancellationToken = default)
    {
        var aluno = await ObterEntidadeAsync(id, cancellationToken);

        if (ativo)
            aluno.Reativar();
        else
            aluno.Desativar();

        await unitOfWork.SalvarAsync(cancellationToken);
    }

    public async Task<AlunoDetalheResponse> VincularResponsavelAsync(Guid id, VinculoResponsavelRequest request, CancellationToken cancellationToken = default)
    {
        var aluno = await ObterEntidadeAsync(id, cancellationToken);

        _ = await responsaveis.ObterAsync(usuarioAtual.MotoristaId, request.ResponsavelId, cancellationToken)
            ?? throw new NaoEncontradoException("Responsável não encontrado.");

        aluno.VincularResponsavel(
            request.ResponsavelId,
            (ParentescoResponsavel)request.Parentesco,
            request.PodeAlterarEndereco,
            request.RecebeNotificacoes);

        await unitOfWork.SalvarAsync(cancellationToken);
        return Detalhar(await ObterEntidadeAsync(id, cancellationToken));
    }

    public async Task<AlunoDetalheResponse> AtualizarVinculoAsync(Guid id, VinculoResponsavelRequest request, CancellationToken cancellationToken = default)
    {
        var aluno = await ObterEntidadeAsync(id, cancellationToken);

        aluno.AtualizarVinculoResponsavel(
            request.ResponsavelId,
            (ParentescoResponsavel)request.Parentesco,
            request.PodeAlterarEndereco,
            request.RecebeNotificacoes);

        await unitOfWork.SalvarAsync(cancellationToken);
        return Detalhar(aluno);
    }

    public async Task DesvincularResponsavelAsync(Guid id, Guid responsavelId, CancellationToken cancellationToken = default)
    {
        var aluno = await ObterEntidadeAsync(id, cancellationToken);
        aluno.DesvincularResponsavel(responsavelId);
        await unitOfWork.SalvarAsync(cancellationToken);
    }

    private static AlunoDetalheResponse Detalhar(Aluno aluno) =>
        new(
            aluno.ParaResposta(),
            aluno.Responsaveis
                .OrderBy(r => r.Responsavel?.Nome)
                .Select(r => r.ParaResposta())
                .ToList());

    private async Task GarantirEscolaAsync(Guid escolaId, CancellationToken cancellationToken)
    {
        _ = await escolas.ObterAsync(usuarioAtual.MotoristaId, escolaId, cancellationToken)
            ?? throw new NaoEncontradoException("Escola não encontrada.");
    }

    private async Task<Aluno> ObterEntidadeAsync(Guid id, CancellationToken cancellationToken)
    {
        var aluno = await alunos.ObterAsync(id, cancellationToken);

        if (aluno is null || aluno.MotoristaId != usuarioAtual.MotoristaId)
            throw new NaoEncontradoException("Aluno não encontrado.");

        return aluno;
    }
}
