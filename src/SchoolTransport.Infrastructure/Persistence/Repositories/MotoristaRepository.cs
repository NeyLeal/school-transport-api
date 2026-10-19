using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Common;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Persistence.Repositories;

public class MotoristaRepository(SchoolTransportDbContext context) : IMotoristaRepository
{
    public Task<Motorista?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.Motoristas
            .Include(x => x.Ajudantes)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Motorista?> ObterPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default) =>
        context.Motoristas.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId, cancellationToken);

    public async Task AdicionarAsync(Motorista motorista, CancellationToken cancellationToken = default) =>
        await context.Motoristas.AddAsync(motorista, cancellationToken);
}
