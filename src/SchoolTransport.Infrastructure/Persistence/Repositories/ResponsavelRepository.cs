using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Common;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Persistence.Repositories;

public class ResponsavelRepository(SchoolTransportDbContext context) : IResponsavelRepository
{
    public Task<List<Responsavel>> ListarAsync(Guid motoristaId, CancellationToken cancellationToken = default) =>
        context.Responsaveis
            .Where(x => x.MotoristaId == motoristaId)
            .OrderBy(x => x.Nome)
            .ToListAsync(cancellationToken);

    public Task<Responsavel?> ObterAsync(Guid motoristaId, Guid id, CancellationToken cancellationToken = default) =>
        context.Responsaveis.FirstOrDefaultAsync(x => x.Id == id && x.MotoristaId == motoristaId, cancellationToken);

    public Task<Responsavel?> ObterPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default) =>
        context.Responsaveis.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId, cancellationToken);

    public async Task AdicionarAsync(Responsavel responsavel, CancellationToken cancellationToken = default) =>
        await context.Responsaveis.AddAsync(responsavel, cancellationToken);
}
