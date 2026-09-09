using FlottaGestionale.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlottaGestionale.Infrastructure.Persistence.Configurations;

public class MezzoConfiguration : IEntityTypeConfiguration<Mezzo>
{
    public void Configure(EntityTypeBuilder<Mezzo> builder)
    {
        builder.ToTable("Mezzi");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Targa).IsRequired().HasMaxLength(15);
        builder.Property(m => m.Marca).IsRequired().HasMaxLength(50);
        builder.Property(m => m.Modello).IsRequired().HasMaxLength(50);
        builder.Property(m => m.Stato).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(m => m.Targa).IsUnique();
    }
}
