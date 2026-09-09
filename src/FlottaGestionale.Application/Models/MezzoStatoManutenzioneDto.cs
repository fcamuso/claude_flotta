using FlottaGestionale.Domain.Entities;
using FlottaGestionale.Domain.Enums;

namespace FlottaGestionale.Application.Models;

public record MezzoStatoManutenzioneDto(Mezzo Mezzo, StatoManutenzione Stato, int KmResidui);
