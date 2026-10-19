using SchoolTransport.Application.Common;
using SchoolTransport.Contracts.Responsaveis;
using SchoolTransport.Domain.Entities;
using SchoolTransport.Domain.Enums;

namespace SchoolTransport.Application.Responsaveis;

public class ResponsavelService(
    IResponsavelRepository responsaveis,
    IUsuarioRepository usuarios,
    ISenhaHasher senhaHasher,
    IUsuarioAtual usuarioAtual,
    IUnitOfWork unitOfWork)
{
    public async Task<IReadOnlyList<ResponsavelResponse>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var lista = await responsaveis.ListarAsync(usuarioAtual.MotoristaId, cancellationToken);
        return lista.Select(r => r.ParaResposta()).ToList();
    }

    public async Task<ResponsavelResponse> ObterAsync(Guid id, CancellationToken cancellationToken = default) =>
        (await ObterEntidadeAsync(id, cancellationToken)).ParaResposta();

    public async Task<ResponsavelResponse> CriarAsync(ResponsavelRequest request, CancellationToken cancellationToken = default)
    {
        var responsavel = new Responsavel(usuarioAtual.MotoristaId, request.Nome, request.Telefone, request.Email);
        await responsaveis.AdicionarAsync(responsavel, cancellationToken);
        await unitOfWork.SalvarAsync(cancellationToken);
        return responsavel.ParaResposta();
    }

    public async Task<ResponsavelResponse> AtualizarAsync(Guid id, ResponsavelRequest request, CancellationToken cancellationToken = default)
    {
        var responsavel = await ObterEntidadeAsync(id, cancellationToken);
        responsavel.Atualizar(request.Nome, request.Telefone, request.Email);
        await unitOfWork.SalvarAsync(cancellationToken);
        return responsavel.ParaResposta();
    }

    public async Task<ResponsavelResponse> CriarAcessoAsync(Guid id, CriarAcessoRequest request, CancellationToken cancellationToken = default)
    {
        Senha.Validar(request.Senha);
        var responsavel = await ObterEntidadeAsync(id, cancellationToken);

        if (await usuarios.EmailEmUsoAsync(responsavel.Email, cancellationToken))
            throw new ConflitoException("Já existe um usuário com este e-mail.");

        var usuario = new Usuario(responsavel.Nome, responsavel.Email, senhaHasher.Gerar(request.Senha), PerfilUsuario.Responsavel);
        responsavel.VincularUsuario(usuario.Id);

        await usuarios.AdicionarAsync(usuario, cancellationToken);
        await unitOfWork.SalvarAsync(cancellationToken);
        return responsavel.ParaResposta();
    }

    private async Task<Responsavel> ObterEntidadeAsync(Guid id, CancellationToken cancellationToken) =>
        await responsaveis.ObterAsync(usuarioAtual.MotoristaId, id, cancellationToken)
            ?? throw new NaoEncontradoException("Responsável não encontrado.");
}
