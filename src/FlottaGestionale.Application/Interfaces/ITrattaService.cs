using FlottaGestionale.Domain.Entities;
using FlottaGestionale.Domain.Enums;

namespace FlottaGestionale.Application.Interfaces;

public interface ITrattaService
{
    Task<List<Tratta>> GetAllAsync(StatoTratta? stato = null, CancellationToken ct = default);
    Task<Tratta?> GetByIdAsync(int id, CancellationToken ct = default);

    // destinazioniOrdinate: id delle Destinazioni nell'ordine in cui vanno visitate (genera le TappaTratta).
    Task<Tratta> CreaTrattaAsync(Tratta tratta, IReadOnlyList<int> destinazioniOrdinate, CancellationToken ct = default);

    Task AvviaTrattaAsync(int trattaId, CancellationToken ct = default);
    Task CompletaTappaAsync(int tappaId, CancellationToken ct = default);

    // Chiude la tratta e aggiorna automaticamente il chilometraggio del mezzo assegnato.
    Task ChiudiTrattaAsync(int trattaId, int kmPercorsi, CancellationToken ct = default);

    Task AnnullaTrattaAsync(int trattaId, CancellationToken ct = default);
}
