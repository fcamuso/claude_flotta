using FlottaGestionale.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlottaGestionale.Infrastructure.Persistence.Configurations;

public class TappaTrattaConfiguration : IEntityTypeConfiguration<TappaTratta>
{
    public void Configure(EntityTypeBuilder<TappaTratta> builder)
    {
        builder.ToTable("TappeTratta");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Stato).HasConversion<string>().HasMaxLength(20);
        builder.Property(t => t.Note).HasMaxLength(500);

        builder.HasOne(t => t.Tratta)
            .WithMany(tr => tr.Tappe)
            .HasForeignKey(t => t.TrattaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Destinazione)
            .WithMany(d => d.Tappe)
            .HasForeignKey(t => t.DestinazioneId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => new { t.TrattaId, t.Ordine }).IsUnique();
    }
}
