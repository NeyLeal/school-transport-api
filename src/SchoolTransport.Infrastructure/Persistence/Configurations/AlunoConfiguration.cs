using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Persistence.Configurations;

public class AlunoConfiguration : AggregateConfiguration<Aluno>
{
    protected override void ConfigurarEntidade(EntityTypeBuilder<Aluno> builder)
    {
        builder.ToTable("Alunos");
        builder.Property(x => x.Nome).HasMaxLength(150).IsRequired();
        builder.Property(x => x.ValorMensalidade).HasPrecision(10, 2);
        builder.Property(x => x.EnderecoPadraoIda).HasMaxLength(500).IsRequired();
        builder.Property(x => x.EnderecoPadraoVolta).HasMaxLength(500).IsRequired();
        builder.HasIndex(x => new { x.MotoristaId, x.Ativo });

        builder.HasOne<Motorista>()
            .WithMany()
            .HasForeignKey(x => x.MotoristaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Escola)
            .WithMany()
            .HasForeignKey(x => x.EscolaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Responsaveis)
            .WithOne()
            .HasForeignKey(x => x.AlunoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Responsaveis).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

public class AlunoResponsavelConfiguration : IEntityTypeConfiguration<AlunoResponsavel>
{
    public void Configure(EntityTypeBuilder<AlunoResponsavel> builder)
    {
        builder.ToTable("AlunoResponsaveis");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasIndex(x => new { x.AlunoId, x.ResponsavelId }).IsUnique();

        builder.HasOne(x => x.Responsavel)
            .WithMany()
            .HasForeignKey(x => x.ResponsavelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
