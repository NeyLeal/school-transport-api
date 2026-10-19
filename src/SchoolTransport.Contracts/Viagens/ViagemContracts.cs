using SchoolTransport.Contracts.Comum;

namespace SchoolTransport.Contracts.Viagens;

public class GerarViagensRequest
{
    public DateOnly Data { get; set; }
}

public class PresencaRequest
{
    public Guid AlunoId { get; set; }
    public DateOnly Data { get; set; }
}

public class AlterarEnderecoRequest
{
    public Guid AlunoId { get; set; }
    public DateOnly Data { get; set; }
    public Trajeto Trajeto { get; set; } = Trajeto.Ida;
    public string Endereco { get; set; } = string.Empty;
}

public record GerarViagensResponse(int Criadas);

public record ViagemResponse(
    Guid Id,
    Guid AlunoId,
    string AlunoNome,
    string EscolaNome,
    DateOnly Data,
    StatusPresenca Status,
    EtapaViagem Etapa,
    string EnderecoIda,
    string EnderecoVolta,
    DateTime? HorarioEntradaVanIda,
    DateTime? HorarioChegadaEscola,
    DateTime? HorarioEntradaVanVolta,
    DateTime? HorarioChegadaEmCasa);

public record MeuAlunoResponse(
    Guid AlunoId,
    string Nome,
    string EscolaNome,
    bool PodeAlterarEndereco,
    ViagemResponse? ViagemDeHoje);
