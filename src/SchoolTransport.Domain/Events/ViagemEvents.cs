namespace SchoolTransport.Domain.Events;

public abstract record ViagemEventBase(Guid ViagemId, Guid AlunoId, DateTime OcorridoEm) : IDomainEvent;

public sealed record AlunoEmbarcouNaVanEvent(Guid ViagemId, Guid AlunoId, DateTime OcorridoEm)
    : ViagemEventBase(ViagemId, AlunoId, OcorridoEm);

public sealed record AlunoChegouNaEscolaEvent(Guid ViagemId, Guid AlunoId, DateTime OcorridoEm)
    : ViagemEventBase(ViagemId, AlunoId, OcorridoEm);

public sealed record AlunoSaiuDaEscolaEvent(Guid ViagemId, Guid AlunoId, DateTime OcorridoEm)
    : ViagemEventBase(ViagemId, AlunoId, OcorridoEm);

public sealed record AlunoChegouEmCasaEvent(Guid ViagemId, Guid AlunoId, DateTime OcorridoEm)
    : ViagemEventBase(ViagemId, AlunoId, OcorridoEm);

public sealed record AusenciaInformadaEvent(Guid ViagemId, Guid AlunoId, DateTime OcorridoEm)
    : ViagemEventBase(ViagemId, AlunoId, OcorridoEm);

public sealed record EnderecoViagemAlteradoEvent(Guid ViagemId, Guid AlunoId, DateTime OcorridoEm)
    : ViagemEventBase(ViagemId, AlunoId, OcorridoEm);
