using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTransport.Domain.Common;

namespace SchoolTransport.Infrastructure.Persistence.Configurations;

public abstract class AggregateConfiguration<T> : IEntityTypeConfiguration<T> where T : AggregateRoot
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Ignore(x => x.Eventos);
        ConfigurarEntidade(builder);
    }

    protected abstract void ConfigurarEntidade(EntityTypeBuilder<T> builder);
}
