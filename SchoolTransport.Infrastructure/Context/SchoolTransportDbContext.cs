using Microsoft.EntityFrameworkCore;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Context
{
    public class SchoolTransportDbContext : DbContext
    {
        public SchoolTransportDbContext(DbContextOptions<SchoolTransportDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SchoolTransportDbContext).Assembly);
        }
    }
}