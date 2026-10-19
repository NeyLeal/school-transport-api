using System.Security.Claims;
using SchoolTransport.Application.Common;
using SchoolTransport.Domain.Enums;
using SchoolTransport.Infrastructure.Security;

namespace SchoolTransport.API.Services;

public class UsuarioAtualHttp(IHttpContextAccessor accessor) : IUsuarioAtual
{
    private ClaimsPrincipal Principal =>
        accessor.HttpContext?.User ?? throw new AcessoNegadoException("Usuário não autenticado.");

    public Guid UsuarioId => ObterGuid(ClaimTypes.NameIdentifier)
        ?? throw new AcessoNegadoException("Usuário não autenticado.");

    public PerfilUsuario Perfil => Enum.TryParse<PerfilUsuario>(Principal.FindFirstValue(ClaimTypes.Role), out var perfil)
        ? perfil
        : throw new AcessoNegadoException("Perfil não identificado.");

    public Guid MotoristaId => ObterGuid(ClaimsPersonalizadas.MotoristaId)
        ?? throw new AcessoNegadoException("Usuário sem motorista vinculado.");

    public Guid? ResponsavelId => ObterGuid(ClaimsPersonalizadas.ResponsavelId);

    private Guid? ObterGuid(string claim) =>
        Guid.TryParse(Principal.FindFirstValue(claim), out var valor) ? valor : null;
}
