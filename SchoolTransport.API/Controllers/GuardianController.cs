using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolTransport.Application.DTOs.Guardians;
using SchoolTransport.Application.Interfaces;
using SchoolTransport.Domain.Entities;
using System.Security.Claims;

namespace SchoolTransport.API.Controllers
{
    [ApiController]
    [Route("guardian")]
    [Authorize]
    public class GuardianController : ControllerBase
    {
        private readonly IGuardianRepository _guardianRepository;
        public GuardianController(IGuardianRepository guardianRepository)
        {
            _guardianRepository = guardianRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateGuardianDto dto)
        {
            var driverId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var guardian = new Guardian(dto.Name, dto.Phone, driverId);
            await _guardianRepository.CreateAsync(guardian);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var driverId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var guardians = await _guardianRepository.GetByDriverAsync(driverId);
            return Ok(guardians);
        }
    }
}
