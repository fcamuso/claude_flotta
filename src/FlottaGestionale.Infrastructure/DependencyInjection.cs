using FlottaGestionale.Application.Interfaces;
using FlottaGestionale.Infrastructure.Identity;
using FlottaGestionale.Infrastructure.Persistence;
using FlottaGestionale.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlottaGestionale.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration["DatabaseProvider"] ?? "Sqlite";
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' non configurata.");

        void ConfigureProvider(DbContextOptionsBuilder options)
        {
            switch (provider)
            {
                case "MySql":
                    // Versione del server MariaDB fissata esplicitamente per evitare che AutoDetect
                    // richieda una connessione live al DB in fase di avvio dell'app.
                    options.UseMySql(
                        connectionString,
                        new MariaDbServerVersion(new Version(10, 11, 0)),
                        b => b.MigrationsAssembly("FlottaGestionale.Infrastructure.MySqlMigrations"));
                    break;
                case "Sqlite":
                default:
                    options.UseSqlite(
                        connectionString,
                        b => b.MigrationsAssembly("FlottaGestionale.Infrastructure.SqliteMigrations"));
                    break;
            }
        }

        // AddDbContextFactory (anziché AddDbContext) perché in Blazor Server il DbContext scoped
        // vive per l'intera durata del circuito (connessione SignalR), non per singola richiesta:
        // riusarlo tra interazioni successive causa conflitti di tracking sulle stessa entità.
        // I Data Access Services creano quindi un DbContext breve per ogni operazione tramite la factory.
        services.AddDbContextFactory<AppDbContext>(ConfigureProvider);

        // ASP.NET Core Identity richiede invece un AppDbContext scoped "classico" (i suoi UserStore/
        // RoleStore sono cablati per riceverlo così). Registrarlo con una seconda AddDbContext
        // indipendente causa un conflitto nel grafo di risoluzione dei servizi (le opzioni finiscono
        // per essere richieste dal provider root anche dentro uno scope): lo deriviamo invece dalla
        // stessa factory, così esiste un'unica fonte di configurazione. Le operazioni di gestione
        // utenti sono occasionali (un amministratore che crea/modifica un account), quindi il rischio
        // di conflitti di tracking discusso sopra per il DbContext scoped è accettabile qui, a
        // differenza dei Data Access Services usati di continuo.
        services.AddScoped<AppDbContext>(sp => sp.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());

        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager()
            // Necessario per GeneratePasswordResetTokenAsync (usato per reimpostare la password di
            // un autista): senza provider di token registrati, Identity non sa come generarli/validarli.
            .AddDefaultTokenProviders();

        services.AddScoped<IAutistaService, AutistaService>();
        services.AddScoped<IMezzoService, MezzoService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IDestinazioneService, DestinazioneService>();
        services.AddScoped<ITrattaService, TrattaService>();
        services.AddScoped<IManutenzioneService, ManutenzioneService>();

        return services;
    }
}
