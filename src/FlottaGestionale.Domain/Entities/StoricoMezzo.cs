using FlottaGestionale.Domain.Enums;

namespace FlottaGestionale.Domain.Entities;

public class StoricoMezzo
{
    public int Id { get; set; }

    public int MezzoId { get; set; }
    public Mezzo Mezzo { get; set; } = null!;

    public TipoEventoStorico TipoEvento { get; set; }
    public DateTime Data { get; set; }
    public int ChilometraggioEvento { get; set; }
    public string Descrizione { get; set; } = string.Empty;
    public decimal? Costo { get; set; }
}
