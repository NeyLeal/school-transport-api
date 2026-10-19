using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : AggregateConfiguration<Usuario>
{
    protected override void ConfigurarEntidade(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        builder.Property(x => x.Nome).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(200).IsRequired();
        builder.Property(x => x.SenhaHash).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Perfil).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
    }
}
