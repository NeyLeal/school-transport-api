using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolTransport.API.Extensions;
using SchoolTransport.Application.Motoristas;
using SchoolTransport.Contracts.Ajudantes;

namespace SchoolTransport.API.Controllers;

[ApiController]
[Route("api/ajudantes")]
[Authorize(Roles = Perfis.Motorista)]
public class AjudantesController(AjudanteService ajudanteService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AjudanteResponse>>> Listar(CancellationToken cancellationToken) =>
        Ok(await ajudanteService.ListarAsync(cancellationToken));

    [HttpPost]
    public async Task<ActionResult<AjudanteResponse>> Adicionar(AjudanteRequest request, CancellationToken cancellationToken) =>
        Ok(await ajudanteService.AdicionarAsync(request, cancellationToken));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remover(Guid id, CancellationToken cancellationToken)
    {
        await ajudanteService.RemoverAsync(id, cancellationToken);
        return NoContent();
    }
}
