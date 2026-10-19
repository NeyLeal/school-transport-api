using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
