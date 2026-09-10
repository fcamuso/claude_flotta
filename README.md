# Gestionale Flotta

Applicazione gestionale per la flotta di mezzi di una ditta di trasporto, sviluppata in **Blazor Server** (.NET 9). Copre anagrafiche (autisti, mezzi, clienti, destinazioni), pianificazione e tracciamento in tempo reale delle tratte, calcolo automatico delle scadenze di manutenzione e autenticazione con ruoli.

Le specifiche complete sono in [PDR.md](PDR.md).

## Stack tecnologico

- **ASP.NET Core Blazor Server** (`InteractiveServer`, nessun WebAssembly)
- **Entity Framework Core** (Code-First) con provider duale:
  - **SQLite** in sviluppo
  - **MariaDB** (Pomelo.EntityFrameworkCore.MySql) in produzione
- **MudBlazor** per l'interfaccia (responsive, mobile-friendly)
- **ASP.NET Core Identity** per autenticazione e ruoli (Admin / Logistica / Autista)

## Architettura

Clean Architecture a progetti separati:

```
src/
  FlottaGestionale.Domain/                    entità di dominio, enum, logica di business pura
  FlottaGestionale.Application/               interfacce dei servizi, DTO
  FlottaGestionale.Infrastructure/             DbContext, configurazioni EF, servizi, Identity
  FlottaGestionale.Infrastructure.SqliteMigrations/   migration EF Core per SQLite
  FlottaGestionale.Infrastructure.MySqlMigrations/    migration EF Core per MariaDB
  FlottaGestionale.Web/                        app Blazor Server (UI, pagine, componenti)
```

Le migration vivono in progetti dedicati per provider: SQLite e MariaDB generano SQL diverso a partire dallo stesso modello, e condividere lo stesso assembly/snapshot causerebbe conflitti tra le due cronologie. Ogni progetto ha una propria `IDesignTimeDbContextFactory` (EF Core scansiona solo l'assembly di startup per trovarla).

## Funzionalità

- **Dashboard operativa** con KPI (mezzi disponibili/in transito/in manutenzione, alert scadenze)
- **CRUD anagrafiche**: Autisti, Mezzi, Clienti, Destinazioni (con ricerca filtrata)
- **Gestione Tratte**: pianificazione (autista + mezzo + cliente + tappe ordinate), avvio, avanzamento tappe in tempo reale, chiusura con aggiornamento automatico del chilometraggio del mezzo
- **Storico Mezzo e alert manutenzione**: calcolo automatico della prossima scadenza in base al chilometraggio, registro interventi (manutenzione/revisione/sinistro)
- **Autenticazione e ruoli**: Admin e Logistica vedono l'intero back office; un Autista vede solo le proprie tratte, individuate automaticamente tramite il collegamento tra il suo account e l'anagrafica
- **Gestione password integrata**: dalla pagina Autisti si crea l'account di accesso di un autista (o se ne reimposta la password) con generazione automatica o manuale, e cambio password obbligato al primo accesso

## Avvio in locale

Prerequisiti: [.NET 9 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run --project src/FlottaGestionale.Web
```

Al primo avvio l'app crea da sola il database SQLite (`flotta.db`, nella cartella del progetto Web) e applica le migration pendenti — non serve alcun passaggio manuale. Il terminale stampa l'indirizzo su cui è in ascolto (es. `http://localhost:5201`, configurabile in `src/FlottaGestionale.Web/Properties/launchSettings.json`).

Accedi con l'utente amministratore creato automaticamente al primo avvio:

- **Email:** `admin@flotta.local`
- **Password:** `Admin123!`

> Questa password è nel codice sorgente ed è quindi pubblica: cambiala subito se usi l'app fuori da un ambiente locale.

## Database di produzione (MariaDB)

In produzione l'app usa MariaDB anziché SQLite, selezionato tramite la configurazione `DatabaseProvider` (vedi `appsettings.json` / variabili d'ambiente). A differenza dello sviluppo, le migration **non** vengono applicate automaticamente all'avvio: vanno eseguite esplicitamente come parte del deploy.

```bash
dotnet ef database update \
  --project src/FlottaGestionale.Infrastructure.MySqlMigrations/FlottaGestionale.Infrastructure.MySqlMigrations.csproj \
  --startup-project src/FlottaGestionale.Infrastructure.MySqlMigrations/FlottaGestionale.Infrastructure.MySqlMigrations.csproj
```

La connection string di default usata dalla design-time factory (`MySqlDesignTimeFactory.cs`) è un segnaposto: sostituiscila con quella reale prima di generare/applicare nuove migration, senza mai committarla.

## Ruoli applicativi

| Ruolo | Accesso |
|---|---|
| **Admin** | Tutto, incluse Utenti e gestione accessi Autisti |
| **Logistica** | Dashboard, anagrafiche, pianificazione tratte, storico mezzi |
| **Autista** | Solo "Le mie tratte" (rilevate automaticamente) e il dettaglio delle proprie tratte |

## Test

Nessuna suite di test automatici al momento: la verifica è stata fatta manualmente in browser (creazione/modifica/eliminazione su ogni modulo, flusso di login/logout/cambio password, chiusura tratta con calcolo km, alert manutenzione).
