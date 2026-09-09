using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FlottaGestionale.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static readonly string[] Ruoli = ["Admin", "Logistica", "Autista"];

    // Crea i ruoli applicativi e un utente Admin predefinito se non esiste ancora nessun utente,
    // così l'app è utilizzabile subito dopo il primo "database update" senza un passaggio manuale.
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var ruolo in Ruoli)
        {
            if (!await roleManager.RoleExistsAsync(ruolo))
            {
                await roleManager.CreateAsync(new IdentityRole(ruolo));
            }
        }

        const string adminEmail = "admin@flotta.local";
        if (await userManager.FindByEmailAsync(adminEmail) is null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
