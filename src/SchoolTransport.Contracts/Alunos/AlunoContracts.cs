using SchoolTransport.Contracts.Comum;

namespace SchoolTransport.Contracts.Alunos;

public class AlunoRequest
{
    public Guid EscolaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateOnly? DataNascimento { get; set; }
    public decimal ValorMensalidade { get; set; }
    public string EnderecoPadraoIda { get; set; } = string.Empty;
    public string EnderecoPadraoVolta { get; set; } = string.Empty;
}

public class VinculoResponsavelRequest
{
    public Guid ResponsavelId { get; set; }
    public Parentesco Parentesco { get; set; } = Parentesco.Mae;
    public bool PodeAlterarEndereco { get; set; } = true;
    public bool RecebeNotificacoes { get; set; } = true;
}

public record AlunoResponse(
    Guid Id,
    string Nome,
    Guid EscolaId,
    string EscolaNome,
    DateOnly? DataNascimento,
    decimal ValorMensalidade,
    string EnderecoPadraoIda,
    string EnderecoPadraoVolta,
    bool Ativo);

public record VinculoResponsavelResponse(
    Guid ResponsavelId,
    string Nome,
    string Telefone,
    Parentesco Parentesco,
    bool PodeAlterarEndereco,
    bool RecebeNotificacoes);

public record AlunoDetalheResponse(AlunoResponse Aluno, IReadOnlyList<VinculoResponsavelResponse> Responsaveis);
