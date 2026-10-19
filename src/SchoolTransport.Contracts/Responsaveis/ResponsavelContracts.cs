namespace SchoolTransport.Contracts.Responsaveis;

public class ResponsavelRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class CriarAcessoRequest
{
    public string Senha { get; set; } = string.Empty;
}

public record ResponsavelResponse(Guid Id, string Nome, string Telefone, string Email, bool PossuiAcesso);
