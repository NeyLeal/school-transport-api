using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolTransport.Application.DTOs.Schools;
using SchoolTransport.Application.Interfaces;
using SchoolTransport.Domain.Entities;
using System.Security.Claims;

namespace SchoolTransport.API.Controllers
{
    [ApiController]
    [Route("school")]
    [Authorize]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolRepository _repository;
        public SchoolController (ISchoolRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSchoolDto dto)
        {
            var driverId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var school = new School(dto.Name, dto.Address, driverId);

            await _repository
                .CreateAsync(school);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var driverId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var schools = await _repository.GetByDriverAsync(driverId);
            return Ok(schools);
        }
    }
}
