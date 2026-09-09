using FlottaGestionale.Domain.Entities;

namespace FlottaGestionale.Application.Interfaces;

public interface IDestinazioneService
{
    Task<List<Destinazione>> GetAllAsync(string? searchTerm = null, CancellationToken ct = default);
    Task<Destinazione?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Destinazione> CreateAsync(Destinazione destinazione, CancellationToken ct = default);
    Task UpdateAsync(Destinazione destinazione, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
