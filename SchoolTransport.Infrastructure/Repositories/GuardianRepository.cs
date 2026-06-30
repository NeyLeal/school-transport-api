using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Interfaces;
using SchoolTransport.Domain.Entities;
using SchoolTransport.Infrastructure.Context;

namespace SchoolTransport.Infrastructure.Repositories
{
    public class GuardianRepository : IGuardianRepository
    {
        private readonly SchoolTransportDbContext _context;

        public GuardianRepository(SchoolTransportDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Guardian guardian)
        {
            await _context.Guardians.AddAsync(guardian);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Guardian>> GetByDriverAsync(Guid driverId)
        {
            return await _context.Guardians
                .Where(x => x.UserId == driverId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
        public async Task<Guardian?> GetByIdAsync(Guid id)
        {
            return await _context.Guardians
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
