using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolTransport.Application.Auth;
using SchoolTransport.Contracts.Auth;
using SchoolTransport.Contracts.Comum;

namespace SchoolTransport.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("registrar-motorista")]
    public async Task<ActionResult<LoginResponse>> RegistrarMotorista(RegistrarMotoristaRequest request, CancellationToken cancellationToken) =>
        Ok(await authService.RegistrarMotoristaAsync(request, cancellationToken));

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken) =>
        Ok(await authService.LoginAsync(request, cancellationToken));

    [Authorize]
    [HttpGet("me")]
    public ActionResult<UsuarioResponse> Me() =>
        Ok(new UsuarioResponse(
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
            User.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
            User.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
            Enum.Parse<Perfil>(User.FindFirstValue(ClaimTypes.Role)!)));
}
