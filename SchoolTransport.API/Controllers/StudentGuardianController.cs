using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolTransport.Application.Interfaces;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.API.Controllers;

[ApiController]
[Route("student")]
[Authorize]
public class StudentGuardianController : ControllerBase
{
    private readonly IStudentGuardianRepository _repository;

    public StudentGuardianController(
        IStudentGuardianRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("{studentId}/guardian/{guardianId}")]
    public async Task<IActionResult> Link(
        Guid studentId,
        Guid guardianId)
    {
        var relation = new StudentGuardian(
            studentId,
            guardianId);

        await _repository.CreateAsync(relation);

        return Ok();
    }

    [HttpGet("{studentId}/guardians")]
    public async Task<IActionResult> Get(Guid studentId)
    {
        var guardians =
            await _repository.GetGuardiansByStudentAsync(studentId);

        return Ok(guardians);
    }

    [HttpDelete("{studentId}/guardian/{guardianId}")]
    public async Task<IActionResult> Delete(
        Guid studentId,
        Guid guardianId)
    {
        var relation =
            await _repository.GetAsync(studentId, guardianId);

        if (relation == null)
            return NotFound();

        await _repository.DeleteAsync(relation);

        return Ok();
    }
}