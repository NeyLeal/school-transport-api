using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Persistence.Configurations;

public class ResponsavelConfiguration : AggregateConfiguration<Responsavel>
{
    protected override void ConfigurarEntidade(EntityTypeBuilder<Responsavel> builder)
    {
        builder.ToTable("Responsaveis");
        builder.Property(x => x.Nome).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Telefone).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(200).IsRequired();
        builder.Ignore(x => x.PossuiAcesso);
        builder.HasIndex(x => x.MotoristaId);
        builder.HasIndex(x => x.UsuarioId).IsUnique().HasFilter("[UsuarioId] IS NOT NULL");

        builder.HasOne<Motorista>()
            .WithMany()
            .HasForeignKey(x => x.MotoristaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
