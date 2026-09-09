using FlottaGestionale.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlottaGestionale.Infrastructure.Persistence.Configurations;

public class StoricoMezzoConfiguration : IEntityTypeConfiguration<StoricoMezzo>
{
    public void Configure(EntityTypeBuilder<StoricoMezzo> builder)
    {
        builder.ToTable("StoricoMezzi");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.TipoEvento).HasConversion<string>().HasMaxLength(20);
        builder.Property(s => s.Descrizione).IsRequired().HasMaxLength(500);
        builder.Property(s => s.Costo).HasColumnType("decimal(10,2)");

        builder.HasOne(s => s.Mezzo)
            .WithMany(m => m.StoricoEventi)
            .HasForeignKey(s => s.MezzoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
