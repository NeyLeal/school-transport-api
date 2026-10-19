using SchoolTransport.Domain.Entities;
using SchoolTransport.Domain.Enums;

namespace SchoolTransport.Application.Common;

public interface IUsuarioAtual
{
    Guid UsuarioId { get; }
    PerfilUsuario Perfil { get; }
    Guid MotoristaId { get; }
    Guid? ResponsavelId { get; }
}

public interface IRelogio
{
    DateTime Agora { get; }
    DateOnly Hoje { get; }
}

public interface ISenhaHasher
{
    string Gerar(string senha);
    bool Verificar(string senha, string hash);
}

public interface ITokenService
{
    TokenGerado Gerar(Usuario usuario, Guid? motoristaId, Guid? responsavelId);
}

public record TokenGerado(string Token, DateTime ExpiraEm);

public interface IUnitOfWork
{
    Task SalvarAsync(CancellationToken cancellationToken = default);
}
