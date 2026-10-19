using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolTransport.API.Extensions;
using SchoolTransport.Application.Escolas;
using SchoolTransport.Contracts.Escolas;

namespace SchoolTransport.API.Controllers;

[ApiController]
[Route("api/escolas")]
[Authorize(Roles = Perfis.Motorista)]
public class EscolasController(EscolaService escolaService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EscolaResponse>>> Listar(CancellationToken cancellationToken) =>
        Ok(await escolaService.ListarAsync(cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EscolaResponse>> Obter(Guid id, CancellationToken cancellationToken) =>
        Ok(await escolaService.ObterAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<EscolaResponse>> Criar(EscolaRequest request, CancellationToken cancellationToken)
    {
        var escola = await escolaService.CriarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Obter), new { id = escola.Id }, escola);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EscolaResponse>> Atualizar(Guid id, EscolaRequest request, CancellationToken cancellationToken) =>
        Ok(await escolaService.AtualizarAsync(id, request, cancellationToken));
}
