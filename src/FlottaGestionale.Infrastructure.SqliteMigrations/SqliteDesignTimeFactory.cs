using FlottaGestionale.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FlottaGestionale.Infrastructure.SqliteMigrations;

// EF Core scansiona solo l'assembly di startup per trovare una IDesignTimeDbContextFactory: per questo
// la factory vive qui (non in FlottaGestionale.Infrastructure) e va usata passando questo stesso
// progetto sia come --project che come --startup-project.
public class SqliteDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite(
            "Data Source=flotta.db",
            b => b.MigrationsAssembly("FlottaGestionale.Infrastructure.SqliteMigrations"));

        return new AppDbContext(optionsBuilder.Options);
    }
}
