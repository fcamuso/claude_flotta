using FlottaGestionale.Infrastructure;
using FlottaGestionale.Infrastructure.Identity;
using FlottaGestionale.Infrastructure.Persistence;
using FlottaGestionale.Web.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddInfrastructure(builder.Configuration);

// SignInManager usa per contratto lo schema "Identity.Application": AddIdentityCookies()
// registra i cookie scheme con i nomi che Identity si aspetta (a differenza di un AddCookie
// generico con nome a piacere, che SignInManager non troverebbe).
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/accesso-negato";
});

builder.Services.AddAuthorization(options =>
{
    // Tutte le pagine richiedono un utente autenticato, a meno che non siano marcate [AllowAnonymous]
    // (es. la pagina di login).
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPost("/logout", async (Microsoft.AspNetCore.Identity.SignInManager<ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.LocalRedirect("/login");
}).RequireAuthorization();

// In sviluppo applica automaticamente le migrazioni pendenti (SQLite locale), così "dotnet run"
// basta da solo per avere un DB pronto. In produzione le migrazioni si applicano esplicitamente
// come parte del deploy, non all'avvio dell'app: vedi le note di deploy.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    await using var db = await dbContextFactory.CreateDbContextAsync();
    await db.Database.MigrateAsync();
}

await IdentitySeeder.SeedAsync(app.Services);

app.Run();
