using FlottaGestionale.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FlottaGestionale.Infrastructure.MySqlMigrations;

// Vedi SqliteDesignTimeFactory nel progetto gemello per il perché questa factory vive qui
// invece che in FlottaGestionale.Infrastructure.
public class MySqlDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(
            "Server=localhost;Port=3306;Database=FlottaGestionale;User=flotta_user;Password=CHANGE_ME;",
            new MariaDbServerVersion(new Version(10, 11, 0)),
            b => b.MigrationsAssembly("FlottaGestionale.Infrastructure.MySqlMigrations"));

        return new AppDbContext(optionsBuilder.Options);
    }
}
