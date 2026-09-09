using FlottaGestionale.Domain.Entities;

namespace FlottaGestionale.Application.Interfaces;

public interface IClienteService
{
    Task<List<Cliente>> GetAllAsync(string? searchTerm = null, CancellationToken ct = default);
    Task<Cliente?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Cliente> CreateAsync(Cliente cliente, CancellationToken ct = default);
    Task UpdateAsync(Cliente cliente, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
