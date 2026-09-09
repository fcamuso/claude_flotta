using FlottaGestionale.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlottaGestionale.Infrastructure.Persistence.Configurations;

public class AutistaConfiguration : IEntityTypeConfiguration<Autista>
{
    public void Configure(EntityTypeBuilder<Autista> builder)
    {
        builder.ToTable("Autisti");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Nome).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Cognome).IsRequired().HasMaxLength(100);
        builder.Property(a => a.CodiceFiscale).HasMaxLength(16);
        builder.Property(a => a.NumeroPatente).IsRequired().HasMaxLength(30);
        builder.Property(a => a.CategoriaPatente).HasMaxLength(10);
        builder.Property(a => a.Telefono).HasMaxLength(30);
        builder.Property(a => a.Email).HasMaxLength(150);

        builder.HasIndex(a => a.CodiceFiscale).IsUnique();
    }
}
