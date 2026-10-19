using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Persistence.Configurations;

public class EscolaConfiguration : AggregateConfiguration<Escola>
{
    protected override void ConfigurarEntidade(EntityTypeBuilder<Escola> builder)
    {
        builder.ToTable("Escolas");
        builder.Property(x => x.Nome).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Endereco).HasMaxLength(500).IsRequired();
        builder.HasIndex(x => x.MotoristaId);

        builder.HasOne<Motorista>()
            .WithMany()
            .HasForeignKey(x => x.MotoristaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
