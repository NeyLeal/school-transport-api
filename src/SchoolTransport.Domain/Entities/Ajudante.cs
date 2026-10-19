using SchoolTransport.Domain.Common;
using SchoolTransport.Domain.Exceptions;

namespace SchoolTransport.Domain.Entities;

public class Ajudante : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;
    public Guid MotoristaId { get; private set; }
    public Guid? UsuarioId { get; private set; }

    private Ajudante() { }

    internal Ajudante(string nome, string telefone, Guid motoristaId)
    {
        Nome = Guard.Obrigatorio(nome, "O nome do ajudante é obrigatório.");
        Telefone = Guard.Obrigatorio(telefone, "O telefone do ajudante é obrigatório.");
        MotoristaId = Guard.Obrigatorio(motoristaId, "O ajudante precisa estar vinculado a um motorista.");
    }

    public bool PossuiAcesso => UsuarioId.HasValue;

    internal void VincularUsuario(Guid usuarioId)
    {
        if (PossuiAcesso)
            throw new DomainException("Este ajudante já possui acesso.");

        UsuarioId = Guard.Obrigatorio(usuarioId, "Usuário inválido.");
        MarcarComoAtualizado();
    }
}
