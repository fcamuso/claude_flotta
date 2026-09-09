using FlottaGestionale.Application.Interfaces;
using FlottaGestionale.Application.Models;
using FlottaGestionale.Domain.Entities;
using FlottaGestionale.Domain.Enums;
using FlottaGestionale.Domain.Services;
using FlottaGestionale.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlottaGestionale.Infrastructure.Services;

public class ManutenzioneService(IDbContextFactory<AppDbContext> dbFactory) : IManutenzioneService
{
    public MezzoStatoManutenzioneDto CalcolaStato(Mezzo mezzo) => new(
        mezzo,
        ManutenzioneCalculator.CalcolaStato(mezzo),
        ManutenzioneCalculator.CalcolaKmResiduiManutenzione(mezzo));

    public async Task<List<MezzoStatoManutenzioneDto>> GetStatoManutenzioneFlottaAsync(CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var mezzi = await db.Mezzi.AsNoTracking().Where(m => m.Attivo).ToListAsync(ct);

        return mezzi
            .Select(CalcolaStato)
            .OrderBy(x => x.KmResidui)
            .ToList();
    }

    public async Task<List<StoricoMezzo>> GetStoricoAsync(int mezzoId, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        return await db.StoricoMezzi
            .AsNoTracking()
            .Where(s => s.MezzoId == mezzoId)
            .OrderByDescending(s => s.Data)
            .ToListAsync(ct);
    }

    public async Task<StoricoMezzo> RegistraEventoAsync(StoricoMezzo evento, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var mezzo = await db.Mezzi.FirstOrDefaultAsync(m => m.Id == evento.MezzoId, ct)
            ?? throw new InvalidOperationException($"Mezzo {evento.MezzoId} non trovato.");

        db.StoricoMezzi.Add(evento);

        if (evento.TipoEvento == TipoEventoStorico.Manutenzione)
        {
            // L'intervento resetta il riferimento km da cui si calcola la prossima scadenza.
            mezzo.ChilometraggioUltimaManutenzione = evento.ChilometraggioEvento;

            if (mezzo.Stato == StatoMezzo.InManutenzione)
            {
                mezzo.Stato = StatoMezzo.Disponibile;
            }
        }

        await db.SaveChangesAsync(ct);
        return evento;
    }
}
