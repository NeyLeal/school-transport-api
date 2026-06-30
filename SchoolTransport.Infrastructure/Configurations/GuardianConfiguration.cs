using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Configurations
{
    public class GuardianConfiguration : IEntityTypeConfiguration<Guardian>
    {
        public void Configure(EntityTypeBuilder<Guardian> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Phone)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}
