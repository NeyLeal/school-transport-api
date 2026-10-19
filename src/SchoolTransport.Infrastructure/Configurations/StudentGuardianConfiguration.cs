using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Configurations
{
    public class StudentGuardianConfiguration : IEntityTypeConfiguration<StudentGuardian>
    {
        public void Configure(EntityTypeBuilder<StudentGuardian> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.Guardian)
                .WithMany()
                .HasForeignKey(x => x.GuardianId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasIndex(x => new
                {
                    x.StudentId,
                    x.GuardianId
                })
                .IsUnique();
        }
    }
}