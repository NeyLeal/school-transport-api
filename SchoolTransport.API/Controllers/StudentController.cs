using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolTransport.Application.DTOs.Students;
using SchoolTransport.Application.Interfaces;
using SchoolTransport.Domain.Entities;
using System.Security.Claims;

namespace SchoolTransport.API.Controllers
{
    [ApiController]
    [Route("student")]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ISchoolRepository _schoolRepository;

        public StudentController(IStudentRepository studentRepository, ISchoolRepository schoolRepository)
        {
            _studentRepository = studentRepository;
            _schoolRepository = schoolRepository;
        }
        [HttpPost]
        public async Task <IActionResult> Create(CreateStudentDto dto)
        {
            var driverId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var school = await _schoolRepository.GetByDriverAsync(dto.SchoolId);
            if(school is null)
            {
                return BadRequest("School not found");
            }
            var student = new Student(dto.Name, dto.BirthDate, driverId, dto.SchoolId, dto.AllowPickupLocationChange);
            await _studentRepository.CreateAsync(student);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var driverId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var students = await _studentRepository.GetByDriverAsync(driverId);
            return Ok(students);
        }
    }
}
