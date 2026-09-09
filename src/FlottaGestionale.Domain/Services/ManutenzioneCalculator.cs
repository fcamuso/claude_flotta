using FlottaGestionale.Domain.Entities;
using FlottaGestionale.Domain.Enums;

namespace FlottaGestionale.Domain.Services;

// Logica pura (nessuna dipendenza da EF Core) per il calcolo delle scadenze di manutenzione
// in base al chilometraggio cumulativo del mezzo. Isolata dal resto per essere riutilizzabile
// sia dai Data Access Services sia, in futuro, da eventuali job schedulati.
public static class ManutenzioneCalculator
{
    public const int SogliaAllertaKm = 1000;

    public static int CalcolaProssimaManutenzioneKm(Mezzo mezzo) =>
        mezzo.ChilometraggioUltimaManutenzione + mezzo.IntervalloManutenzioneKm;

    public static int CalcolaKmResiduiManutenzione(Mezzo mezzo) =>
        CalcolaProssimaManutenzioneKm(mezzo) - mezzo.ChilometraggioAttuale;

    public static StatoManutenzione CalcolaStato(Mezzo mezzo)
    {
        var kmResidui = CalcolaKmResiduiManutenzione(mezzo);

        if (kmResidui <= 0)
        {
            return StatoManutenzione.Scaduta;
        }

        return kmResidui <= SogliaAllertaKm
            ? StatoManutenzione.Imminente
            : StatoManutenzione.InRegola;
    }
}
