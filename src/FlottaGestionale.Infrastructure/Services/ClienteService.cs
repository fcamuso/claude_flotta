using FlottaGestionale.Application.Interfaces;
using FlottaGestionale.Domain.Entities;
using FlottaGestionale.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlottaGestionale.Infrastructure.Services;

public class ClienteService(IDbContextFactory<AppDbContext> dbFactory) : IClienteService
{
    public async Task<List<Cliente>> GetAllAsync(string? searchTerm = null, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var query = db.Clienti.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = $"%{searchTerm.Trim()}%";
            query = query.Where(c =>
                EF.Functions.Like(c.RagioneSociale, term) ||
                (c.PartitaIva != null && EF.Functions.Like(c.PartitaIva, term)));
        }

        return await query.OrderBy(c => c.RagioneSociale).ToListAsync(ct);
    }

    public async Task<Cliente?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        return await db.Clienti.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<Cliente> CreateAsync(Cliente cliente, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        db.Clienti.Add(cliente);
        await db.SaveChangesAsync(ct);
        return cliente;
    }

    public async Task UpdateAsync(Cliente cliente, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        db.Clienti.Update(cliente);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var cliente = await db.Clienti.FindAsync([id], ct)
            ?? throw new InvalidOperationException($"Cliente {id} non trovato.");
        db.Clienti.Remove(cliente);
        await db.SaveChangesAsync(ct);
    }
}
