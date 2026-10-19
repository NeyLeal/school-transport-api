namespace SchoolTransport.Contracts.Ajudantes;

public class AjudanteRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
}

public class CriarAcessoAjudanteRequest
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public record AjudanteResponse(Guid Id, string Nome, string Telefone, bool PossuiAcesso);
