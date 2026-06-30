using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Interfaces;
using SchoolTransport.Domain.Entities;
using SchoolTransport.Infrastructure.Context;

namespace SchoolTransport.Infrastructure.Repositories
{
    public class SchoolRepository : ISchoolRepository
    {
        private readonly SchoolTransportDbContext _context;

        public SchoolRepository(SchoolTransportDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(School school)
        {
            await _context.Schools.AddAsync(school);
            await _context.SaveChangesAsync();
        }
        public async Task<List<School>> GetByDriverAsync(Guid driverId)
        {
            return await _context.Schools
                .Where(x => x.DriverId == driverId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
        public async Task<School?> GetByIdAsync(Guid id)
        {
            return await _context.Schools
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
