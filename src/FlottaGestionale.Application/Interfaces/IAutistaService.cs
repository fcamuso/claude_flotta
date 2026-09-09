using FlottaGestionale.Domain.Entities;

namespace FlottaGestionale.Application.Interfaces;

public interface IAutistaService
{
    Task<List<Autista>> GetAllAsync(string? searchTerm = null, CancellationToken ct = default);
    Task<Autista?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Autista> CreateAsync(Autista autista, CancellationToken ct = default);
    Task UpdateAsync(Autista autista, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
