using FlottaGestionale.Application.Models;
using FlottaGestionale.Domain.Entities;

namespace FlottaGestionale.Application.Interfaces;

public interface IManutenzioneService
{
    MezzoStatoManutenzioneDto CalcolaStato(Mezzo mezzo);

    // Stato di manutenzione di tutti i mezzi attivi, ordinato per urgenza (km residui crescenti).
    Task<List<MezzoStatoManutenzioneDto>> GetStatoManutenzioneFlottaAsync(CancellationToken ct = default);

    Task<List<StoricoMezzo>> GetStoricoAsync(int mezzoId, CancellationToken ct = default);

    // Registra un intervento (manutenzione/revisione/sinistro); se è una Manutenzione, aggiorna
    // automaticamente il chilometraggio di riferimento del mezzo e lo riporta Disponibile.
    Task<StoricoMezzo> RegistraEventoAsync(StoricoMezzo evento, CancellationToken ct = default);
}
