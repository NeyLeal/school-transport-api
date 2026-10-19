using SchoolTransport.Application.Common;
using SchoolTransport.Contracts.Ajudantes;
using SchoolTransport.Domain.Entities;
using SchoolTransport.Domain.Enums;

namespace SchoolTransport.Application.Motoristas;

public class AjudanteService(
    IMotoristaRepository motoristas,
    IUsuarioRepository usuarios,
    ISenhaHasher senhaHasher,
    IUsuarioAtual usuarioAtual,
    IUnitOfWork unitOfWork)
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

    public async Task<AjudanteResponse> CriarAcessoAsync(Guid ajudanteId, CriarAcessoAjudanteRequest request, CancellationToken cancellationToken = default)
    {
        Senha.Validar(request.Senha);
        var email = Senha.NormalizarEmail(request.Email);
        var motorista = await ObterMotoristaAsync(cancellationToken);
        var ajudante = motorista.Ajudantes.FirstOrDefault(a => a.Id == ajudanteId)
            ?? throw new NaoEncontradoException("Ajudante não encontrado.");

        if (await usuarios.EmailEmUsoAsync(email, cancellationToken))
            throw new ConflitoException("Já existe um usuário com este e-mail.");

        var usuario = new Usuario(ajudante.Nome, email, senhaHasher.Gerar(request.Senha), PerfilUsuario.Ajudante);
        motorista.LiberarAcessoAjudante(ajudante.Id, usuario.Id);

        await usuarios.AdicionarAsync(usuario, cancellationToken);
        await unitOfWork.SalvarAsync(cancellationToken);
        return ajudante.ParaResposta();
    }

    public async Task RemoverAsync(Guid ajudanteId, CancellationToken cancellationToken = default)
    {
        var motorista = await ObterMotoristaAsync(cancellationToken);
        var ajudante = motorista.RemoverAjudante(ajudanteId);

        // Remove o login junto: libera o e-mail para um novo cadastro e corta o acesso imediatamente.
        if (ajudante.UsuarioId is { } usuarioId && await usuarios.ObterPorIdAsync(usuarioId, cancellationToken) is { } usuario)
            usuarios.Remover(usuario);

        await unitOfWork.SalvarAsync(cancellationToken);
    }

    private async Task<Motorista> ObterMotoristaAsync(CancellationToken cancellationToken) =>
        await motoristas.ObterPorIdAsync(usuarioAtual.MotoristaId, cancellationToken)
            ?? throw new NaoEncontradoException("Motorista não encontrado.");
}
