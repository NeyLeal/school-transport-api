using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTransport.Infrastructure.Outbox;

namespace SchoolTransport.Infrastructure.Persistence.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Tipo).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Conteudo).IsRequired();
        builder.Property(x => x.Erro).HasMaxLength(2000);
        builder.HasIndex(x => new { x.ProcessadoEm, x.OcorridoEm });
    }
}
