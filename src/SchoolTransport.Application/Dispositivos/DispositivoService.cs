using SchoolTransport.Application.Common;
using SchoolTransport.Contracts.Dispositivos;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Application.Dispositivos;

public class DispositivoService(IDispositivoPushRepository dispositivos, IUsuarioAtual usuarioAtual, IUnitOfWork unitOfWork)
{
    public async Task RegistrarAsync(RegistrarDispositivoRequest request, CancellationToken cancellationToken = default)
    {
        var token = request.Token?.Trim() ?? string.Empty;
        var dispositivo = await dispositivos.ObterPorTokenAsync(token, cancellationToken);

        // O mesmo aparelho pode trocar de usuário (logout/login); o token passa a ser de quem entrou por último.
        if (dispositivo is null)
            await dispositivos.AdicionarAsync(new DispositivoPush(usuarioAtual.UsuarioId, token, request.Plataforma), cancellationToken);
        else
            dispositivo.VincularA(usuarioAtual.UsuarioId, request.Plataforma);

        await unitOfWork.SalvarAsync(cancellationToken);
    }

    public async Task RemoverAsync(RemoverDispositivoRequest request, CancellationToken cancellationToken = default)
    {
        var dispositivo = await dispositivos.ObterPorTokenAsync(request.Token?.Trim() ?? string.Empty, cancellationToken);

        if (dispositivo is null || dispositivo.UsuarioId != usuarioAtual.UsuarioId)
            return;

        dispositivos.Remover(dispositivo);
        await unitOfWork.SalvarAsync(cancellationToken);
    }
}
