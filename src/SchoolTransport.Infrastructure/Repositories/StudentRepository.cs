using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Interfaces;
using SchoolTransport.Domain.Entities;
using SchoolTransport.Infrastructure.Context;

namespace SchoolTransport.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolTransportDbContext _context;

        public StudentRepository(SchoolTransportDbContext context) 
        {
            _context = context;
        }
        public async Task CreateAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Student>> GetByDriverAsync(Guid driverId)
        {
            return await _context.Students
                .Include(x => x.School)
                .Where(x => x.DriverId == driverId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await _context.Students
                .Include(x => x.School)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
