using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolTransport.Application.Dispositivos;
using SchoolTransport.Contracts.Dispositivos;

namespace SchoolTransport.API.Controllers;

[ApiController]
[Route("api/dispositivos")]
[Authorize]
public class DispositivosController(DispositivoService dispositivoService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Registrar(RegistrarDispositivoRequest request, CancellationToken cancellationToken)
    {
        await dispositivoService.RegistrarAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpPost("remover")]
    public async Task<IActionResult> Remover(RemoverDispositivoRequest request, CancellationToken cancellationToken)
    {
        await dispositivoService.RemoverAsync(request, cancellationToken);
        return NoContent();
    }
}
