using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Persistence.Configurations;

public class MotoristaConfiguration : AggregateConfiguration<Motorista>
{
    protected override void ConfigurarEntidade(EntityTypeBuilder<Motorista> builder)
    {
        builder.ToTable("Motoristas");
        builder.Property(x => x.Nome).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Cnh).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Telefone).HasMaxLength(30).IsRequired();
        builder.HasIndex(x => x.UsuarioId).IsUnique();

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Ajudantes)
            .WithOne()
            .HasForeignKey(x => x.MotoristaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Ajudantes).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

public class AjudanteConfiguration : IEntityTypeConfiguration<Ajudante>
{
    public void Configure(EntityTypeBuilder<Ajudante> builder)
    {
        builder.ToTable("Ajudantes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Nome).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Telefone).HasMaxLength(30).IsRequired();
    }
}
