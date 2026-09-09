namespace FlottaGestionale.Domain.Entities;

public class Destinazione
{
    public int Id { get; set; }
    public string Denominazione { get; set; } = string.Empty;
    public string Indirizzo { get; set; } = string.Empty;
    public string? Citta { get; set; }
    public string? Cap { get; set; }
    public string? Provincia { get; set; }
    public string? Note { get; set; }
    public bool Attivo { get; set; } = true;

    public ICollection<TappaTratta> Tappe { get; set; } = new List<TappaTratta>();
}
