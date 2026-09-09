using FlottaGestionale.Application.Interfaces;
using FlottaGestionale.Domain.Entities;
using FlottaGestionale.Domain.Enums;
using FlottaGestionale.Domain.Services;
using FlottaGestionale.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlottaGestionale.Infrastructure.Services;

public class TrattaService(IDbContextFactory<AppDbContext> dbFactory) : ITrattaService
{
    public async Task<List<Tratta>> GetAllAsync(StatoTratta? stato = null, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var query = db.Tratte
            .Include(t => t.Autista)
            .Include(t => t.Mezzo)
            .Include(t => t.Cliente)
            .Include(t => t.Tappe)
            .AsNoTracking()
            .AsQueryable();

        if (stato is not null)
        {
            query = query.Where(t => t.Stato == stato);
        }

        return await query.OrderByDescending(t => t.DataPianificata).ToListAsync(ct);
    }

    public async Task<Tratta?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        return await db.Tratte
            .Include(t => t.Autista)
            .Include(t => t.Mezzo)
            .Include(t => t.Cliente)
            .Include(t => t.Tappe).ThenInclude(tp => tp.Destinazione)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<Tratta> CreaTrattaAsync(Tratta tratta, IReadOnlyList<int> destinazioniOrdinate, CancellationToken ct = default)
    {
        if (destinazioniOrdinate.Count == 0)
        {
            throw new InvalidOperationException("Una tratta deve avere almeno una tappa.");
        }

        for (var i = 0; i < destinazioniOrdinate.Count; i++)
        {
            tratta.Tappe.Add(new TappaTratta { DestinazioneId = destinazioniOrdinate[i], Ordine = i + 1 });
        }

        await using var db = await dbFactory.CreateDbContextAsync(ct);
        db.Tratte.Add(tratta);
        await db.SaveChangesAsync(ct);
        return tratta;
    }

    public async Task AvviaTrattaAsync(int trattaId, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var tratta = await db.Tratte.Include(t => t.Mezzo).FirstOrDefaultAsync(t => t.Id == trattaId, ct)
            ?? throw new InvalidOperationException($"Tratta {trattaId} non trovata.");

        if (tratta.Stato != StatoTratta.Pianificata)
        {
            throw new InvalidOperationException("Solo una tratta pianificata può essere avviata.");
        }

        tratta.Stato = StatoTratta.InCorso;
        tratta.DataInizio = DateTime.UtcNow;
        tratta.Mezzo.Stato = StatoMezzo.InTransito;

        await db.SaveChangesAsync(ct);
    }

    public async Task CompletaTappaAsync(int tappaId, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var tappa = await db.TappeTratta.FirstOrDefaultAsync(t => t.Id == tappaId, ct)
            ?? throw new InvalidOperationException($"Tappa {tappaId} non trovata.");

        tappa.Stato = StatoTappa.Completata;
        tappa.DataOraCompletamento = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
    }

    public async Task ChiudiTrattaAsync(int trattaId, int kmPercorsi, CancellationToken ct = default)
    {
        if (kmPercorsi < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(kmPercorsi), "I km percorsi non possono essere negativi.");
        }

        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var tratta = await db.Tratte.Include(t => t.Mezzo).FirstOrDefaultAsync(t => t.Id == trattaId, ct)
            ?? throw new InvalidOperationException($"Tratta {trattaId} non trovata.");

        if (tratta.Stato != StatoTratta.InCorso)
        {
            throw new InvalidOperationException("Solo una tratta in corso può essere chiusa.");
        }

        tratta.Stato = StatoTratta.Completata;
        tratta.DataFine = DateTime.UtcNow;
        tratta.KmPercorsi = kmPercorsi;

        // Aggiornamento automatico del chilometraggio del mezzo alla chiusura della tratta.
        tratta.Mezzo.ChilometraggioAttuale += kmPercorsi;

        // Se la percorrenza ha fatto scattare la scadenza di manutenzione, il mezzo passa
        // automaticamente "In manutenzione" invece di tornare "Disponibile".
        tratta.Mezzo.Stato = ManutenzioneCalculator.CalcolaStato(tratta.Mezzo) == StatoManutenzione.Scaduta
            ? StatoMezzo.InManutenzione
            : StatoMezzo.Disponibile;

        await db.SaveChangesAsync(ct);
    }

    public async Task AnnullaTrattaAsync(int trattaId, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var tratta = await db.Tratte.Include(t => t.Mezzo).FirstOrDefaultAsync(t => t.Id == trattaId, ct)
            ?? throw new InvalidOperationException($"Tratta {trattaId} non trovata.");

        if (tratta.Stato == StatoTratta.Completata)
        {
            throw new InvalidOperationException("Una tratta completata non può essere annullata.");
        }

        tratta.Stato = StatoTratta.Annullata;

        if (tratta.Mezzo.Stato == StatoMezzo.InTransito)
        {
            tratta.Mezzo.Stato = StatoMezzo.Disponibile;
        }

        await db.SaveChangesAsync(ct);
    }
}
