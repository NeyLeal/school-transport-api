using SchoolTransport.Contracts.Comum;

namespace SchoolTransport.Contracts.Auth;

public class RegistrarMotoristaRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Cnh { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public record UsuarioResponse(Guid Id, string Nome, string Email, Perfil Perfil);

public record LoginResponse(string Token, DateTime ExpiraEm, UsuarioResponse Usuario);
