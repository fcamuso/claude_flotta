namespace FlottaGestionale.Domain.Entities;

public class Autista
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public string? CodiceFiscale { get; set; }
    public string NumeroPatente { get; set; } = string.Empty;
    public string? CategoriaPatente { get; set; }
    public DateOnly? DataScadenzaPatente { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public bool Attivo { get; set; } = true;

    public ICollection<Tratta> Tratte { get; set; } = new List<Tratta>();
}
