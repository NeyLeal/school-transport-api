using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Common;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Persistence.Repositories;

public class EscolaRepository(SchoolTransportDbContext context) : IEscolaRepository
{
    public Task<List<Escola>> ListarAsync(Guid motoristaId, CancellationToken cancellationToken = default) =>
        context.Escolas
            .Where(x => x.MotoristaId == motoristaId)
            .OrderBy(x => x.Nome)
            .ToListAsync(cancellationToken);

    public Task<Escola?> ObterAsync(Guid motoristaId, Guid id, CancellationToken cancellationToken = default) =>
        context.Escolas.FirstOrDefaultAsync(x => x.Id == id && x.MotoristaId == motoristaId, cancellationToken);

    public async Task AdicionarAsync(Escola escola, CancellationToken cancellationToken = default) =>
        await context.Escolas.AddAsync(escola, cancellationToken);
}
