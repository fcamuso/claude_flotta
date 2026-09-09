using FlottaGestionale.Domain.Enums;

namespace FlottaGestionale.Domain.Entities;

public class Tratta
{
    public int Id { get; set; }

    public int AutistaId { get; set; }
    public Autista Autista { get; set; } = null!;

    public int MezzoId { get; set; }
    public Mezzo Mezzo { get; set; } = null!;

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;

    public DateTime DataPianificata { get; set; }
    public DateTime? DataInizio { get; set; }
    public DateTime? DataFine { get; set; }
    public int? KmPercorsi { get; set; }

    public StatoTratta Stato { get; set; } = StatoTratta.Pianificata;
    public string? Note { get; set; }

    public ICollection<TappaTratta> Tappe { get; set; } = new List<TappaTratta>();
}
