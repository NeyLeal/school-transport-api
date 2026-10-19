using SchoolTransport.Domain.Common;

namespace SchoolTransport.Domain.Entities;

public class Escola : AggregateRoot
{
    public Guid MotoristaId { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Endereco { get; private set; } = string.Empty;

    private Escola() { }

    public Escola(Guid motoristaId, string nome, string endereco)
    {
        MotoristaId = Guard.Obrigatorio(motoristaId, "A escola precisa estar vinculada a um motorista.");
        Nome = Guard.Obrigatorio(nome, "O nome da escola é obrigatório.");
        Endereco = Guard.Obrigatorio(endereco, "O endereço da escola é obrigatório.");
    }

    public void Atualizar(string nome, string endereco)
    {
        Nome = Guard.Obrigatorio(nome, "O nome da escola é obrigatório.");
        Endereco = Guard.Obrigatorio(endereco, "O endereço da escola é obrigatório.");
        MarcarComoAtualizado();
    }
}
