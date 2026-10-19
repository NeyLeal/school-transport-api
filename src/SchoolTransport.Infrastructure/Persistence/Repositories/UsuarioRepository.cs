using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Common;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Persistence.Repositories;

public class UsuarioRepository(SchoolTransportDbContext context) : IUsuarioRepository
{
    public Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default) =>
        context.Usuarios.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public Task<bool> EmailEmUsoAsync(string email, CancellationToken cancellationToken = default) =>
        context.Usuarios.AnyAsync(x => x.Email == email, cancellationToken);

    public async Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken = default) =>
        await context.Usuarios.AddAsync(usuario, cancellationToken);
}
