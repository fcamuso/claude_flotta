using FlottaGestionale.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlottaGestionale.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasOne(u => u.Autista)
            .WithMany()
            .HasForeignKey(u => u.AutistaId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
