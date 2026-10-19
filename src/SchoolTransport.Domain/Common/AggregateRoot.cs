using SchoolTransport.Domain.Events;

namespace SchoolTransport.Domain.Common;

public abstract class AggregateRoot : BaseEntity
{
    private readonly List<IDomainEvent> _eventos = [];

    public IReadOnlyCollection<IDomainEvent> Eventos => _eventos.AsReadOnly();

    protected void AdicionarEvento(IDomainEvent evento)
    {
        _eventos.Add(evento);
    }

    public void LimparEventos()
    {
        _eventos.Clear();
    }
}
