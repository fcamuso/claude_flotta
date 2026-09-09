using FlottaGestionale.Application.Interfaces;
using FlottaGestionale.Domain.Entities;
using FlottaGestionale.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlottaGestionale.Infrastructure.Services;

public class MezzoService(IDbContextFactory<AppDbContext> dbFactory) : IMezzoService
{
    public async Task<List<Mezzo>> GetAllAsync(string? searchTerm = null, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var query = db.Mezzi.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = $"%{searchTerm.Trim()}%";
            query = query.Where(m =>
                EF.Functions.Like(m.Targa, term) ||
                EF.Functions.Like(m.Marca, term) ||
                EF.Functions.Like(m.Modello, term));
        }

        return await query.OrderBy(m => m.Targa).ToListAsync(ct);
    }

    public async Task<Mezzo?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        return await db.Mezzi.FirstOrDefaultAsync(m => m.Id == id, ct);
    }

    public async Task<Mezzo> CreateAsync(Mezzo mezzo, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        db.Mezzi.Add(mezzo);
        await db.SaveChangesAsync(ct);
        return mezzo;
    }

    public async Task UpdateAsync(Mezzo mezzo, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        db.Mezzi.Update(mezzo);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var mezzo = await db.Mezzi.FindAsync([id], ct)
            ?? throw new InvalidOperationException($"Mezzo {id} non trovato.");
        db.Mezzi.Remove(mezzo);
        await db.SaveChangesAsync(ct);
    }
}
