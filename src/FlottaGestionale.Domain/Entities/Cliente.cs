namespace FlottaGestionale.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }
    public string RagioneSociale { get; set; } = string.Empty;
    public string? PartitaIva { get; set; }
    public string? Indirizzo { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public bool Attivo { get; set; } = true;

    public ICollection<Tratta> Tratte { get; set; } = new List<Tratta>();
}
