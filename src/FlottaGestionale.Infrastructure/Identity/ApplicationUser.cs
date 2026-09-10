using FlottaGestionale.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FlottaGestionale.Infrastructure.Identity;

// Utente applicativo. Il collegamento opzionale ad Autista permette di derivare automaticamente
// "quali tratte sono le mie" per un utente con ruolo Autista, senza selezione manuale.
public class ApplicationUser : IdentityUser
{
    public int? AutistaId { get; set; }
    public Autista? Autista { get; set; }

    // Impostato quando un amministratore crea l'account o reimposta la password: forza l'utente
    // a sceglierne una nuova al primo accesso, applicato via middleware (vedi Program.cs).
    public bool MustChangePassword { get; set; }
}
