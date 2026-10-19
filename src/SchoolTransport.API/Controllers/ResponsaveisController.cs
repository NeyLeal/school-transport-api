using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolTransport.API.Extensions;
using SchoolTransport.Application.Responsaveis;
using SchoolTransport.Contracts.Responsaveis;

namespace SchoolTransport.API.Controllers;

[ApiController]
[Route("api/responsaveis")]
[Authorize(Roles = Perfis.Motorista)]
public class ResponsaveisController(ResponsavelService responsavelService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ResponsavelResponse>>> Listar(CancellationToken cancellationToken) =>
        Ok(await responsavelService.ListarAsync(cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ResponsavelResponse>> Obter(Guid id, CancellationToken cancellationToken) =>
        Ok(await responsavelService.ObterAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<ResponsavelResponse>> Criar(ResponsavelRequest request, CancellationToken cancellationToken)
    {
        var responsavel = await responsavelService.CriarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Obter), new { id = responsavel.Id }, responsavel);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ResponsavelResponse>> Atualizar(Guid id, ResponsavelRequest request, CancellationToken cancellationToken) =>
        Ok(await responsavelService.AtualizarAsync(id, request, cancellationToken));

    [HttpPost("{id:guid}/acesso")]
    public async Task<ActionResult<ResponsavelResponse>> CriarAcesso(Guid id, CriarAcessoRequest request, CancellationToken cancellationToken) =>
        Ok(await responsavelService.CriarAcessoAsync(id, request, cancellationToken));
}
