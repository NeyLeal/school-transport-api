using SchoolTransport.Domain.Common;
using SchoolTransport.Domain.Exceptions;

namespace SchoolTransport.Domain.Entities;

public class Responsavel : AggregateRoot
{
    public Guid MotoristaId { get; private set; }
    public Guid? UsuarioId { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    private Responsavel() { }

    public Responsavel(Guid motoristaId, string nome, string telefone, string email)
    {
        MotoristaId = Guard.Obrigatorio(motoristaId, "O responsável precisa estar vinculado a um motorista.");
        Nome = Guard.Obrigatorio(nome, "O nome do responsável é obrigatório.");
        Telefone = Guard.Obrigatorio(telefone, "O telefone do responsável é obrigatório.");
        Email = Guard.Email(email, "E-mail do responsável inválido.");
    }

    public bool PossuiAcesso => UsuarioId.HasValue;

    public void Atualizar(string nome, string telefone, string email)
    {
        if (PossuiAcesso && !string.Equals(Email, email?.Trim(), StringComparison.OrdinalIgnoreCase))
            throw new DomainException("Não é possível alterar o e-mail de um responsável com acesso ativo.");

        Nome = Guard.Obrigatorio(nome, "O nome do responsável é obrigatório.");
        Telefone = Guard.Obrigatorio(telefone, "O telefone do responsável é obrigatório.");
        Email = Guard.Email(email, "E-mail do responsável inválido.");
        MarcarComoAtualizado();
    }

    public void VincularUsuario(Guid usuarioId)
    {
        if (PossuiAcesso)
            throw new DomainException("Este responsável já possui acesso.");

        UsuarioId = Guard.Obrigatorio(usuarioId, "Usuário inválido.");
        MarcarComoAtualizado();
    }
}
