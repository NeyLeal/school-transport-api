using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolTransport.API.Extensions;
using SchoolTransport.Application.Viagens;
using SchoolTransport.Contracts.Comum;
using SchoolTransport.Contracts.Viagens;

namespace SchoolTransport.API.Controllers;

[ApiController]
[Route("api/viagens")]
public class ViagensController(ViagemService viagemService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = Perfis.Equipe)]
    public async Task<ActionResult<IReadOnlyList<ViagemResponse>>> Listar([FromQuery] DateOnly? data, CancellationToken cancellationToken) =>
        Ok(await viagemService.ListarAsync(data, cancellationToken));

    [HttpPost("gerar")]
    [Authorize(Roles = Perfis.Motorista)]
    public async Task<ActionResult<GerarViagensResponse>> Gerar(GerarViagensRequest request, CancellationToken cancellationToken) =>
        Ok(await viagemService.GerarAsync(request, cancellationToken));

    [HttpPost("{id:guid}/acoes/{acao}")]
    [Authorize(Roles = Perfis.Equipe)]
    public async Task<ActionResult<ViagemResponse>> Registrar(Guid id, AcaoViagem acao, CancellationToken cancellationToken) =>
        Ok(await viagemService.RegistrarAsync(id, acao, cancellationToken));

    [HttpPost("ausencia")]
    [Authorize(Roles = Perfis.MotoristaOuResponsavel)]
    public async Task<ActionResult<ViagemResponse>> InformarAusencia(PresencaRequest request, CancellationToken cancellationToken) =>
        Ok(await viagemService.InformarAusenciaAsync(request, cancellationToken));

    [HttpPost("presenca")]
    [Authorize(Roles = Perfis.MotoristaOuResponsavel)]
    public async Task<ActionResult<ViagemResponse>> ConfirmarPresenca(PresencaRequest request, CancellationToken cancellationToken) =>
        Ok(await viagemService.ConfirmarPresencaAsync(request, cancellationToken));

    [HttpPut("endereco")]
    [Authorize(Roles = Perfis.MotoristaOuResponsavel)]
    public async Task<ActionResult<ViagemResponse>> AlterarEndereco(AlterarEnderecoRequest request, CancellationToken cancellationToken) =>
        Ok(await viagemService.AlterarEnderecoAsync(request, cancellationToken));

    [HttpGet("meus-alunos")]
    [Authorize(Roles = Perfis.Responsavel)]
    public async Task<ActionResult<IReadOnlyList<MeuAlunoResponse>>> MeusAlunos(CancellationToken cancellationToken) =>
        Ok(await viagemService.ListarMeusAlunosAsync(cancellationToken));
}
