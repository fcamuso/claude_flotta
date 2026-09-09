using FlottaGestionale.Application.Interfaces;
using FlottaGestionale.Domain.Entities;
using FlottaGestionale.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlottaGestionale.Infrastructure.Services;

public class DestinazioneService(IDbContextFactory<AppDbContext> dbFactory) : IDestinazioneService
{
    public async Task<List<Destinazione>> GetAllAsync(string? searchTerm = null, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var query = db.Destinazioni.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = $"%{searchTerm.Trim()}%";
            query = query.Where(d =>
                EF.Functions.Like(d.Denominazione, term) ||
                (d.Citta != null && EF.Functions.Like(d.Citta, term)));
        }

        return await query.OrderBy(d => d.Denominazione).ToListAsync(ct);
    }

    public async Task<Destinazione?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        return await db.Destinazioni.FirstOrDefaultAsync(d => d.Id == id, ct);
    }

    public async Task<Destinazione> CreateAsync(Destinazione destinazione, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        db.Destinazioni.Add(destinazione);
        await db.SaveChangesAsync(ct);
        return destinazione;
    }

    public async Task UpdateAsync(Destinazione destinazione, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        db.Destinazioni.Update(destinazione);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var destinazione = await db.Destinazioni.FindAsync([id], ct)
            ?? throw new InvalidOperationException($"Destinazione {id} non trovata.");
        db.Destinazioni.Remove(destinazione);
        await db.SaveChangesAsync(ct);
    }
}
