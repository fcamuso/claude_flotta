using FlottaGestionale.Application.Interfaces;
using FlottaGestionale.Domain.Entities;
using FlottaGestionale.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlottaGestionale.Infrastructure.Services;

public class AutistaService(IDbContextFactory<AppDbContext> dbFactory) : IAutistaService
{
    public async Task<List<Autista>> GetAllAsync(string? searchTerm = null, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var query = db.Autisti.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = $"%{searchTerm.Trim()}%";
            query = query.Where(a =>
                EF.Functions.Like(a.Nome, term) ||
                EF.Functions.Like(a.Cognome, term) ||
                (a.CodiceFiscale != null && EF.Functions.Like(a.CodiceFiscale, term)));
        }

        return await query.OrderBy(a => a.Cognome).ThenBy(a => a.Nome).ToListAsync(ct);
    }

    public async Task<Autista?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        return await db.Autisti.FirstOrDefaultAsync(a => a.Id == id, ct);
    }

    public async Task<Autista> CreateAsync(Autista autista, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        db.Autisti.Add(autista);
        await db.SaveChangesAsync(ct);
        return autista;
    }

    public async Task UpdateAsync(Autista autista, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        db.Autisti.Update(autista);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var autista = await db.Autisti.FindAsync([id], ct)
            ?? throw new InvalidOperationException($"Autista {id} non trovato.");
        db.Autisti.Remove(autista);
        await db.SaveChangesAsync(ct);
    }
}
