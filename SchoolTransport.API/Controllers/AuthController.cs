using Microsoft.AspNetCore.Mvc;

using SchoolTransport.Application.DTOs.Auth;
using SchoolTransport.Application.Interfaces;
using SchoolTransport.Application.Security;
using SchoolTransport.Domain.Entities;
using SchoolTransport.Domain.Enums;

namespace SchoolTransport.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    private readonly IJwtService _jwtService;

    public AuthController(
        IUserRepository userRepository,
        IJwtService jwtService)
    {
        _userRepository =
            userRepository;

        _jwtService =
            jwtService;
    }

    [HttpPost("register-driver")]
    public async Task<IActionResult>
        RegisterDriver(
            RegisterDriverDto dto)
    {
        var exists =
            await _userRepository
                .GetByEmailAsync(
                    dto.Email);

        if (exists is not null)
        {
            return BadRequest(
                "Email já cadastrado");
        }

        var passwordHash = PasswordHasher.Hash(dto.Password);

        var user =
            new User(dto.Name, dto.Email, passwordHash, UserRole.Driver, true, null, null);

        await _userRepository
            .CreateAsync(
                user);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult>
        Login(
            LoginDto dto)
    {
        var user =
            await _userRepository
                .GetByEmailAsync(
                    dto.Email);

        if (user is null)
        {
            return Unauthorized();
        }

        var validPassword =
            PasswordHasher.Verify(
                dto.Password,
                user.PasswordHash);

        if (!validPassword)
        {
            return Unauthorized();
        }

        var token =
            _jwtService
                .GenerateToken(
                    user);

        return Ok(
            new
            {
                token
            });
    }
}