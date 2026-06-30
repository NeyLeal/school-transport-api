using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Interfaces;
using SchoolTransport.Domain.Entities;
using SchoolTransport.Infrastructure.Context;

namespace SchoolTransport.Infrastructure.Repositories
{
    public class StudentGuardianRpository : IStudentGuardianRepository
    {
        private readonly SchoolTransportDbContext _context;
        public StudentGuardianRpository(SchoolTransportDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(StudentGuardian relation)
        {
            await _context.StudentGuardians.AddAsync(relation);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Guardian>> GetGuardiansByStudentAsync(Guid studentId)
        {
            return await _context.StudentGuardians
                .Where(x => x.StudentId == studentId)
                .Include(x => x.Guardian)
                .Select(x => x.Guardian!)
                .ToListAsync();
        }
        public async Task<StudentGuardian?> GetAsync(Guid studentId, Guid guardianId)
        {
            return await _context.StudentGuardians.FirstOrDefaultAsync(X => X.StudentId == studentId && X.GuardianId == guardianId);
        }
        public async Task DeleteAsync(StudentGuardian relation)
        {
            _context.StudentGuardians.Remove(relation);
            await _context.SaveChangesAsync();
        }
    }
}
