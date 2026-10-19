using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Common;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Persistence.Repositories;

public class DispositivoPushRepository(SchoolTransportDbContext context) : IDispositivoPushRepository
{
    public Task<DispositivoPush?> ObterPorTokenAsync(string token, CancellationToken cancellationToken = default) =>
        context.DispositivosPush.FirstOrDefaultAsync(x => x.Token == token, cancellationToken);

    public Task<List<DispositivoPush>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default) =>
        context.DispositivosPush.Where(x => x.UsuarioId == usuarioId).ToListAsync(cancellationToken);

    public async Task AdicionarAsync(DispositivoPush dispositivo, CancellationToken cancellationToken = default) =>
        await context.DispositivosPush.AddAsync(dispositivo, cancellationToken);

    public void Remover(DispositivoPush dispositivo) =>
        context.DispositivosPush.Remove(dispositivo);
}
