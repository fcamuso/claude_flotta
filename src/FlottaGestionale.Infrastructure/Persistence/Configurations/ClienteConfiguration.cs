using FlottaGestionale.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlottaGestionale.Infrastructure.Persistence.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clienti");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.RagioneSociale).IsRequired().HasMaxLength(200);
        builder.Property(c => c.PartitaIva).HasMaxLength(20);
        builder.Property(c => c.Indirizzo).HasMaxLength(250);
        builder.Property(c => c.Telefono).HasMaxLength(30);
        builder.Property(c => c.Email).HasMaxLength(150);
    }
}
