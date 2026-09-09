using FlottaGestionale.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlottaGestionale.Infrastructure.Persistence.Configurations;

public class TrattaConfiguration : IEntityTypeConfiguration<Tratta>
{
    public void Configure(EntityTypeBuilder<Tratta> builder)
    {
        builder.ToTable("Tratte");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Stato).HasConversion<string>().HasMaxLength(20);
        builder.Property(t => t.Note).HasMaxLength(500);

        builder.HasOne(t => t.Autista)
            .WithMany(a => a.Tratte)
            .HasForeignKey(t => t.AutistaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Mezzo)
            .WithMany(m => m.Tratte)
            .HasForeignKey(t => t.MezzoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Cliente)
            .WithMany(c => c.Tratte)
            .HasForeignKey(t => t.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
