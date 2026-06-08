using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Interfaces;
using SchoolTransport.Domain.Entities;
using SchoolTransport.Infrastructure.Context;

namespace SchoolTransport.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SchoolTransportDbContext _context;

        public UserRepository(SchoolTransportDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync( x => x.Id == id);
        }
        public async Task CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}