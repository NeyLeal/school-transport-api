using Microsoft.EntityFrameworkCore;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Context
{
    public class SchoolTransportDbContext : DbContext
    {
        public SchoolTransportDbContext(DbContextOptions<SchoolTransportDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<School> Schools => Set<School>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Guardian> Guardians => Set<Guardian>();
        public DbSet<StudentGuardian> StudentGuardians => Set<StudentGuardian>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SchoolTransportDbContext).Assembly);
        }
    }
}