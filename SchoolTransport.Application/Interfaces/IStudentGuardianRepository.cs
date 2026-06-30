using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Application.Interfaces
{
    public interface IStudentGuardianRepository
    {
        Task CreateAsync(StudentGuardian relation);
        Task<List<Guardian>> GetGuardiansByStudentAsync(Guid studentId);
        Task<StudentGuardian?> GetAsync(Guid studentId, Guid guardianId);
        Task DeleteAsync(StudentGuardian relation);
    }
}
