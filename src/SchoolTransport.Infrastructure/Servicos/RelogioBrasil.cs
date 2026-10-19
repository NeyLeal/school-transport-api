using SchoolTransport.Application.Common;

namespace SchoolTransport.Infrastructure.Servicos;

public class RelogioBrasil(TimeProvider timeProvider) : IRelogio
{
    private static readonly TimeZoneInfo Fuso = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

    public DateTime Agora => TimeZoneInfo.ConvertTimeFromUtc(timeProvider.GetUtcNow().UtcDateTime, Fuso);

    public DateOnly Hoje => DateOnly.FromDateTime(Agora);
}
