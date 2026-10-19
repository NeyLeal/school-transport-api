using SchoolTransport.Application.Common;
using SchoolTransport.Contracts.Comum;
using SchoolTransport.Contracts.Viagens;
using SchoolTransport.Domain.Entities;
using SchoolTransport.Domain.Enums;
using SchoolTransport.Domain.Exceptions;
using TrajetoDominio = SchoolTransport.Domain.Enums.Trajeto;

namespace SchoolTransport.Application.Viagens;

public class ViagemService(
    IViagemRepository viagens,
    IAlunoRepository alunos,
    IMotoristaRepository motoristas,
    IUsuarioAtual usuarioAtual,
    IRelogio relogio,
    IUnitOfWork unitOfWork,
    IPublicadorViagens publicador)
{
    public async Task<IReadOnlyList<ViagemResponse>> ListarAsync(DateOnly? data, CancellationToken cancellationToken = default)
    {
        await GarantirAjudanteAtivoAsync(cancellationToken);
        var lista = await viagens.ListarPorDataAsync(usuarioAtual.MotoristaId, data ?? relogio.Hoje, cancellationToken);
        return lista.Select(v => v.ParaResposta()).ToList();
    }

    public async Task<GerarViagensResponse> GerarAsync(GerarViagensRequest request, CancellationToken cancellationToken = default)
    {
        GarantirDataNaoPassada(request.Data);

        var ativos = await alunos.ListarAsync(usuarioAtual.MotoristaId, incluirInativos: false, cancellationToken);
        var comViagem = await viagens.AlunosComViagemAsync(usuarioAtual.MotoristaId, request.Data, cancellationToken);

        var novas = ativos
            .Where(a => !comViagem.Contains(a.Id))
            .Select(a => new ViagemAluno(a.Id, a.MotoristaId, request.Data))
            .ToList();

        if (novas.Count > 0)
        {
            await viagens.AdicionarVariasAsync(novas, cancellationToken);
            await unitOfWork.SalvarAsync(cancellationToken);
        }

        return new GerarViagensResponse(novas.Count);
    }

    public async Task<ViagemResponse> RegistrarAsync(Guid id, AcaoViagem acao, CancellationToken cancellationToken = default)
    {
        await GarantirAjudanteAtivoAsync(cancellationToken);
        var viagem = await viagens.ObterAsync(id, cancellationToken);

        if (viagem is null || viagem.MotoristaId != usuarioAtual.MotoristaId)
            throw new NaoEncontradoException("Viagem não encontrada.");

        switch (acao)
        {
            case AcaoViagem.Embarque:
                viagem.RegistrarEmbarque();
                break;
            case AcaoViagem.ChegadaEscola:
                viagem.RegistrarChegadaNaEscola();
                break;
            case AcaoViagem.SaidaEscola:
                viagem.RegistrarSaidaDaEscola();
                break;
            case AcaoViagem.ChegadaCasa:
                viagem.RegistrarChegadaEmCasa();
                break;
            default:
                throw new DomainException("Ação de viagem inválida.");
        }

        return await SalvarEPublicarAsync(viagem, cancellationToken);
    }

    public async Task<ViagemResponse> InformarAusenciaAsync(PresencaRequest request, CancellationToken cancellationToken = default)
    {
        var viagem = await ObterOuCriarViagemAsync(request.AlunoId, request.Data, exigirPermissaoEndereco: false, cancellationToken);
        viagem.InformarAusencia();
        return await SalvarEPublicarAsync(viagem, cancellationToken);
    }

    public async Task<ViagemResponse> ConfirmarPresencaAsync(PresencaRequest request, CancellationToken cancellationToken = default)
    {
        var viagem = await ObterOuCriarViagemAsync(request.AlunoId, request.Data, exigirPermissaoEndereco: false, cancellationToken);
        viagem.ConfirmarPresenca();
        return await SalvarEPublicarAsync(viagem, cancellationToken);
    }

    public async Task<ViagemResponse> AlterarEnderecoAsync(AlterarEnderecoRequest request, CancellationToken cancellationToken = default)
    {
        var viagem = await ObterOuCriarViagemAsync(request.AlunoId, request.Data, exigirPermissaoEndereco: true, cancellationToken);
        viagem.AlterarEndereco((TrajetoDominio)request.Trajeto, request.Endereco, relogio.Agora);
        return await SalvarEPublicarAsync(viagem, cancellationToken);
    }

    public async Task<IReadOnlyList<MeuAlunoResponse>> ListarMeusAlunosAsync(CancellationToken cancellationToken = default)
    {
        var responsavelId = usuarioAtual.ResponsavelId
            ?? throw new AcessoNegadoException("Disponível apenas para responsáveis.");

        var lista = await alunos.ListarPorResponsavelAsync(responsavelId, cancellationToken);
        var viagensDeHoje = await viagens.ListarPorAlunosAsync(lista.Select(a => a.Id), relogio.Hoje, cancellationToken);

        return lista
            .Select(a => new MeuAlunoResponse(
                a.Id,
                a.Nome,
                a.Escola?.Nome ?? string.Empty,
                a.Responsaveis.Any(r => r.ResponsavelId == responsavelId && r.PodeAlterarEndereco),
                viagensDeHoje.FirstOrDefault(v => v.AlunoId == a.Id)?.ParaResposta()))
            .ToList();
    }

    // O token do ajudante continua válido após ele ser removido da equipe; confere o vínculo a cada uso.
    private async Task GarantirAjudanteAtivoAsync(CancellationToken cancellationToken)
    {
        if (usuarioAtual.Perfil != PerfilUsuario.Ajudante)
            return;

        var motorista = await motoristas.ObterPorUsuarioDoAjudanteAsync(usuarioAtual.UsuarioId, cancellationToken);

        if (motorista?.Id != usuarioAtual.MotoristaId)
            throw new AcessoNegadoException("Você não faz mais parte desta equipe.");
    }

    private async Task<ViagemResponse> SalvarEPublicarAsync(ViagemAluno viagem, CancellationToken cancellationToken)
    {
        await unitOfWork.SalvarAsync(cancellationToken);

        var resposta = viagem.ParaResposta();
        await publicador.ViagemAtualizadaAsync(viagem.MotoristaId, resposta, cancellationToken);
        return resposta;
    }

    private async Task<ViagemAluno> ObterOuCriarViagemAsync(Guid alunoId, DateOnly data, bool exigirPermissaoEndereco, CancellationToken cancellationToken)
    {
        GarantirDataNaoPassada(data);

        var aluno = await alunos.ObterAsync(alunoId, cancellationToken)
            ?? throw new NaoEncontradoException("Aluno não encontrado.");

        GarantirAcesso(aluno, exigirPermissaoEndereco);

        var viagem = await viagens.ObterPorAlunoEDataAsync(alunoId, data, cancellationToken);

        if (viagem is not null)
            return viagem;

        viagem = new ViagemAluno(aluno.Id, aluno.MotoristaId, data);
        await viagens.AdicionarAsync(viagem, cancellationToken);
        return viagem;
    }

    private void GarantirAcesso(Aluno aluno, bool exigirPermissaoEndereco)
    {
        if (usuarioAtual.Perfil == PerfilUsuario.Responsavel)
        {
            var responsavelId = usuarioAtual.ResponsavelId
                ?? throw new AcessoNegadoException("Responsável não identificado.");

            if (exigirPermissaoEndereco)
                aluno.GarantirPermissaoAlteracaoEndereco(responsavelId);
            else
                aluno.GarantirResponsavelVinculado(responsavelId);

            return;
        }

        if (aluno.MotoristaId != usuarioAtual.MotoristaId)
            throw new NaoEncontradoException("Aluno não encontrado.");
    }

    private void GarantirDataNaoPassada(DateOnly data)
    {
        if (data < relogio.Hoje)
            throw new DomainException("Não é possível alterar viagens de datas passadas.");
    }
}
