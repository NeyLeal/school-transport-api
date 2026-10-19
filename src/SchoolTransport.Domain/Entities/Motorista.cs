using SchoolTransport.Domain.Common;
using SchoolTransport.Domain.Exceptions;

namespace SchoolTransport.Domain.Entities;

public class Motorista : AggregateRoot
{
    public Guid UsuarioId { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Cnh { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;

    private readonly List<Ajudante> _ajudantes = [];
    public IReadOnlyCollection<Ajudante> Ajudantes => _ajudantes.AsReadOnly();

    private Motorista() { }

    public Motorista(Guid usuarioId, string nome, string cnh, string telefone)
    {
        UsuarioId = Guard.Obrigatorio(usuarioId, "O motorista precisa estar vinculado a um usuário.");
        Nome = Guard.Obrigatorio(nome, "O nome do motorista é obrigatório.");
        Cnh = Guard.Obrigatorio(cnh, "A CNH do motorista é obrigatória.");
        Telefone = Guard.Obrigatorio(telefone, "O telefone do motorista é obrigatório.");
    }

    public void AtualizarDados(string nome, string telefone)
    {
        Nome = Guard.Obrigatorio(nome, "O nome do motorista é obrigatório.");
        Telefone = Guard.Obrigatorio(telefone, "O telefone do motorista é obrigatório.");
        MarcarComoAtualizado();
    }

    public Ajudante AdicionarAjudante(string nome, string telefone)
    {
        var ajudante = new Ajudante(nome, telefone, Id);
        _ajudantes.Add(ajudante);
        MarcarComoAtualizado();
        return ajudante;
    }

    public Ajudante LiberarAcessoAjudante(Guid ajudanteId, Guid usuarioId)
    {
        var ajudante = ObterAjudante(ajudanteId);
        ajudante.VincularUsuario(usuarioId);
        MarcarComoAtualizado();
        return ajudante;
    }

    public Ajudante RemoverAjudante(Guid ajudanteId)
    {
        var ajudante = ObterAjudante(ajudanteId);
        _ajudantes.Remove(ajudante);
        MarcarComoAtualizado();
        return ajudante;
    }

    private Ajudante ObterAjudante(Guid ajudanteId) =>
        _ajudantes.FirstOrDefault(a => a.Id == ajudanteId)
            ?? throw new DomainException("Ajudante não encontrado para este motorista.");
}
