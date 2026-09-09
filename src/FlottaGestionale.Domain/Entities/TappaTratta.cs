using FlottaGestionale.Domain.Enums;

namespace FlottaGestionale.Domain.Entities;

public class TappaTratta
{
    public int Id { get; set; }

    public int TrattaId { get; set; }
    public Tratta Tratta { get; set; } = null!;

    public int DestinazioneId { get; set; }
    public Destinazione Destinazione { get; set; } = null!;

    public int Ordine { get; set; }
    public StatoTappa Stato { get; set; } = StatoTappa.DaCompletare;
    public DateTime? DataOraCompletamento { get; set; }
    public string? Note { get; set; }
}
