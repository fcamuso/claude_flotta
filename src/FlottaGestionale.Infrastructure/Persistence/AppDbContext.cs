using FlottaGestionale.Domain.Entities;
using FlottaGestionale.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlottaGestionale.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Autista> Autisti => Set<Autista>();
    public DbSet<Mezzo> Mezzi => Set<Mezzo>();
    public DbSet<Cliente> Clienti => Set<Cliente>();
    public DbSet<Destinazione> Destinazioni => Set<Destinazione>();
    public DbSet<Tratta> Tratte => Set<Tratta>();
    public DbSet<TappaTratta> TappeTratta => Set<TappaTratta>();
    public DbSet<StoricoMezzo> StoricoMezzi => Set<StoricoMezzo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
