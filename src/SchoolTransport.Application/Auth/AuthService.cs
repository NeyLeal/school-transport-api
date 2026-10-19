using SchoolTransport.Application.Common;
using SchoolTransport.Contracts.Auth;
using SchoolTransport.Domain.Entities;
using SchoolTransport.Domain.Enums;

namespace SchoolTransport.Application.Auth;

public class AuthService(
    IUsuarioRepository usuarios,
    IMotoristaRepository motoristas,
    IResponsavelRepository responsaveis,
    ISenhaHasher senhaHasher,
    ITokenService tokenService,
    IUnitOfWork unitOfWork)
{
    public async Task<LoginResponse> RegistrarMotoristaAsync(RegistrarMotoristaRequest request, CancellationToken cancellationToken = default)
    {
        Senha.Validar(request.Senha);
        var email = Senha.NormalizarEmail(request.Email);

        if (await usuarios.EmailEmUsoAsync(email, cancellationToken))
            throw new ConflitoException("E-mail já cadastrado.");

        var usuario = new Usuario(request.Nome, email, senhaHasher.Gerar(request.Senha), PerfilUsuario.Motorista);
        var motorista = new Motorista(usuario.Id, request.Nome, request.Cnh, request.Telefone);

        await usuarios.AdicionarAsync(usuario, cancellationToken);
        await motoristas.AdicionarAsync(motorista, cancellationToken);
        await unitOfWork.SalvarAsync(cancellationToken);

        return GerarResposta(usuario, motorista.Id, null);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var usuario = await usuarios.ObterPorEmailAsync(Senha.NormalizarEmail(request.Email), cancellationToken);

        if (usuario is null || !usuario.Ativo || !senhaHasher.Verificar(request.Senha, usuario.SenhaHash))
            throw new CredenciaisInvalidasException();

        switch (usuario.Perfil)
        {
            case PerfilUsuario.Motorista:
                var motorista = await motoristas.ObterPorUsuarioAsync(usuario.Id, cancellationToken)
                    ?? throw new CredenciaisInvalidasException();
                return GerarResposta(usuario, motorista.Id, null);

            case PerfilUsuario.Responsavel:
                var responsavel = await responsaveis.ObterPorUsuarioAsync(usuario.Id, cancellationToken)
                    ?? throw new CredenciaisInvalidasException();
                return GerarResposta(usuario, responsavel.MotoristaId, responsavel.Id);

            case PerfilUsuario.Ajudante:
                var motoristaDoAjudante = await motoristas.ObterPorUsuarioDoAjudanteAsync(usuario.Id, cancellationToken)
                    ?? throw new CredenciaisInvalidasException();
                return GerarResposta(usuario, motoristaDoAjudante.Id, null);

            default:
                return GerarResposta(usuario, null, null);
        }
    }

    private LoginResponse GerarResposta(Usuario usuario, Guid? motoristaId, Guid? responsavelId)
    {
        var token = tokenService.Gerar(usuario, motoristaId, responsavelId);
        return new LoginResponse(token.Token, token.ExpiraEm, usuario.ParaResposta());
    }
}
