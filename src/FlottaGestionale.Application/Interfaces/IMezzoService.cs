using FlottaGestionale.Domain.Entities;

namespace FlottaGestionale.Application.Interfaces;

public interface IMezzoService
{
    Task<List<Mezzo>> GetAllAsync(string? searchTerm = null, CancellationToken ct = default);
    Task<Mezzo?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Mezzo> CreateAsync(Mezzo mezzo, CancellationToken ct = default);
    Task UpdateAsync(Mezzo mezzo, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
