using FlottaGestionale.Domain.Enums;

namespace FlottaGestionale.Domain.Entities;

public class Mezzo
{
    public int Id { get; set; }
    public string Targa { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Modello { get; set; } = string.Empty;
    public int? Anno { get; set; }

    public int ChilometraggioAttuale { get; set; }
    public int IntervalloManutenzioneKm { get; set; } = 15000;
    public int ChilometraggioUltimaManutenzione { get; set; }

    public StatoMezzo Stato { get; set; } = StatoMezzo.Disponibile;
    public bool Attivo { get; set; } = true;

    public ICollection<Tratta> Tratte { get; set; } = new List<Tratta>();
    public ICollection<StoricoMezzo> StoricoEventi { get; set; } = new List<StoricoMezzo>();
}
