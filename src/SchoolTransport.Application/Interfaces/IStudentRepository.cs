using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Application.Interfaces
{
    public interface IStudentRepository
    {
        Task CreateAsync(Student student);
        Task<List<Student>> GetByDriverAsync(Guid drivetId);
        Task<Student?> GetByIdAsync(Guid id);
    }
}
