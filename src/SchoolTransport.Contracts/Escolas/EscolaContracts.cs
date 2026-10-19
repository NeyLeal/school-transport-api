namespace SchoolTransport.Contracts.Escolas;

public class EscolaRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
}

public record EscolaResponse(Guid Id, string Nome, string Endereco);
