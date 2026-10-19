using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolTransport.API.Extensions;
using SchoolTransport.Application.Alunos;
using SchoolTransport.Contracts.Alunos;

namespace SchoolTransport.API.Controllers;

[ApiController]
[Route("api/alunos")]
[Authorize(Roles = Perfis.Motorista)]
public class AlunosController(AlunoService alunoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AlunoResponse>>> Listar([FromQuery] bool incluirInativos, CancellationToken cancellationToken) =>
        Ok(await alunoService.ListarAsync(incluirInativos, cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AlunoDetalheResponse>> Obter(Guid id, CancellationToken cancellationToken) =>
        Ok(await alunoService.ObterAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<AlunoResponse>> Criar(AlunoRequest request, CancellationToken cancellationToken)
    {
        var aluno = await alunoService.CriarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Obter), new { id = aluno.Id }, aluno);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AlunoResponse>> Atualizar(Guid id, AlunoRequest request, CancellationToken cancellationToken) =>
        Ok(await alunoService.AtualizarAsync(id, request, cancellationToken));

    [HttpPost("{id:guid}/desativar")]
    public async Task<IActionResult> Desativar(Guid id, CancellationToken cancellationToken)
    {
        await alunoService.DefinirAtivoAsync(id, false, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/reativar")]
    public async Task<IActionResult> Reativar(Guid id, CancellationToken cancellationToken)
    {
        await alunoService.DefinirAtivoAsync(id, true, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/responsaveis")]
    public async Task<ActionResult<AlunoDetalheResponse>> VincularResponsavel(Guid id, VinculoResponsavelRequest request, CancellationToken cancellationToken) =>
        Ok(await alunoService.VincularResponsavelAsync(id, request, cancellationToken));

    [HttpPut("{id:guid}/responsaveis")]
    public async Task<ActionResult<AlunoDetalheResponse>> AtualizarVinculo(Guid id, VinculoResponsavelRequest request, CancellationToken cancellationToken) =>
        Ok(await alunoService.AtualizarVinculoAsync(id, request, cancellationToken));

    [HttpDelete("{id:guid}/responsaveis/{responsavelId:guid}")]
    public async Task<IActionResult> DesvincularResponsavel(Guid id, Guid responsavelId, CancellationToken cancellationToken)
    {
        await alunoService.DesvincularResponsavelAsync(id, responsavelId, cancellationToken);
        return NoContent();
    }
}
