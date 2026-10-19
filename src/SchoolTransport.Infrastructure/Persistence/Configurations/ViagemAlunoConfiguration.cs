using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Persistence.Configurations;

public class ViagemAlunoConfiguration : AggregateConfiguration<ViagemAluno>
{
    protected override void ConfigurarEntidade(EntityTypeBuilder<ViagemAluno> builder)
    {
        builder.ToTable("Viagens");
        builder.Property(x => x.EnderecoIdaAtualizado).HasMaxLength(500);
        builder.Property(x => x.EnderecoVoltaAtualizado).HasMaxLength(500);
        builder.HasIndex(x => new { x.AlunoId, x.Data }).IsUnique();
        builder.HasIndex(x => new { x.MotoristaId, x.Data });

        builder.HasOne(x => x.Aluno)
            .WithMany()
            .HasForeignKey(x => x.AlunoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Motorista>()
            .WithMany()
            .HasForeignKey(x => x.MotoristaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
