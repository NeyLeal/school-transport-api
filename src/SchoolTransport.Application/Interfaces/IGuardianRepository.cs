using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Application.Interfaces
{
    public interface IGuardianRepository
    {
        Task CreateAsync(Guardian guardian);
        Task<List<Guardian>> GetByDriverAsync(Guid driverId);
        Task<Guardian?> GetByIdAsync(Guid id);
    }
}
