using FlottaGestionale.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlottaGestionale.Infrastructure.Persistence.Configurations;

public class DestinazioneConfiguration : IEntityTypeConfiguration<Destinazione>
{
    public void Configure(EntityTypeBuilder<Destinazione> builder)
    {
        builder.ToTable("Destinazioni");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Denominazione).IsRequired().HasMaxLength(200);
        builder.Property(d => d.Indirizzo).IsRequired().HasMaxLength(250);
        builder.Property(d => d.Citta).HasMaxLength(100);
        builder.Property(d => d.Cap).HasMaxLength(10);
        builder.Property(d => d.Provincia).HasMaxLength(2);
        builder.Property(d => d.Note).HasMaxLength(500);
    }
}
