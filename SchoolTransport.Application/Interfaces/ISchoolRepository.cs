using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Application.Interfaces
{
    public interface ISchoolRepository
    {
        Task CreateAsync(School school);
        Task<List<School>> GetByDriverAsync(Guid driverId);
        Task<School?> GetByIdAsync(Guid id);
    }
}
