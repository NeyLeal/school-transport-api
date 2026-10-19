namespace SchoolTransport.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTime CriadoEm { get; protected set; }
    public DateTime? AtualizadoEm { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CriadoEm = DateTime.UtcNow;
    }

    protected void MarcarComoAtualizado()
    {
        AtualizadoEm = DateTime.UtcNow;
    }
}
