using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Persistence.Configurations;

public class DispositivoPushConfiguration : AggregateConfiguration<DispositivoPush>
{
    protected override void ConfigurarEntidade(EntityTypeBuilder<DispositivoPush> builder)
    {
        builder.ToTable("DispositivosPush");
        builder.Property(x => x.Token).HasMaxLength(512).IsRequired();
        builder.Property(x => x.Plataforma).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => x.Token).IsUnique();
        builder.HasIndex(x => x.UsuarioId);

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
