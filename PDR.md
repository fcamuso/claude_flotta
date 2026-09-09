# PDR - Gestionale Flotta Blazor Server

## Istruzioni per l'Agente (Mindset & Execution)
Operi direttamente sul file system locale tramite CLI.  
Per ogni step del piano operativo devi:
1. Eseguire i comandi CLI necessari (`dotnet new`, `dotnet add package`, `dotnet ef`, ecc.) per creare o aggiornare la struttura.
2. Scrivere e creare direttamente i file di codice (`.cs`, `.razor`, `.csproj`, `appsettings.json`) nel file system.
3. Verificare la compilazione del progetto con `dotnet build`.
4. Al termine dei comandi e delle modifiche relative al singolo step, fornire un sintetico recap dei file creati/modificati e attendere la conferma dell'utente prima di procedere con lo step successivo.

---

## Ruolo e Metodologia di Lavoro

Sei un **Senior Software Architect e Sviluppatore C# esperto in ASP.NET Core Blazor**.

### Regole Tassative
Il tuo output non deve mai essere un blocco massivo di codice. Devi procedere seguendo rigorosamente un piano operativo a step.  
Alla fine di ogni step, devi fornire un breve recap di quanto fatto e fermarti in attesa di una mia conferma esplicita prima di passare allo step successivo, scrivere altro codice o fornire spiegazioni anticipate.

---

## Obiettivo
Sviluppare un gestionale per la flotta di mezzi di una ditta di trasporto.

---

## Stack Tecnologico e Vincoli
* **Framework:** C# ASP.NET Core, Blazor Server (`InteractiveServer`, NO WebAssembly/WASM).
* **IDE di Riferimento:** Visual Studio 18.
* **Architettura:** Scaffolding moderno e modulare (Clean Architecture o Vertical Slice).
* **UI/UX:** Responsive e mobile-friendly (es. MudBlazor o Radzen) per consentire l'uso agevole anche da smartphone/tablet da parte degli autisti.
* **Database:** Entity Framework Core (Code-First) con configurazione duale:
  * **Development:** SQLite locale.
  * **Production:** MariaDB (provider `Pomelo.EntityFrameworkCore.MySql`).

---

## Specifiche Funzionali e Dominio

### 1. Entità Principali (CRUD Completo)
* **Autisti:** Inserimento, ricerca filtrata, modifica, eliminazione.
* **Mezzi:** Gestione anagrafica con indicazione del chilometraggio attuale e soglie di manutenzione.
* **Destinazioni:** Gestione punti di consegna e logistica.
* **Clienti:** Anagrafica clienti della ditta.

### 2. Gestione Tratte, Tappe e Tracciamento in Tempo Reale
* Assegnazione di una consegna/tratta a un Autista, Mezzo e Cliente.
* Gestione tappe intermedie della tratta in ordine sequenziale.
* Interfaccia operatore (desktop) per la pianificazione delle consegne.
* Interfaccia autista (mobile-friendly) per avanzamento tratta e marcatura tappe completate in tempo reale.
* Chiusura tratta con aggiornamento automatico dei chilometri percorsi sul mezzo.

### 3. Logica Automatica di Manutenzione e Storico Mezzi
* Calcolo automatico delle scadenze di manutenzione/tagliando basato sul chilometraggio cumulativo registrato al completamento delle tratte.
* Alert e notifiche visive per manutenzioni imminenti o superate.
* Registro storico del mezzo: interventi di manutenzione, revisioni, incidenti/sinistri e storico tratte/autisti.

### 4. Integrazioni Architetturali
* Autenticazione e Ruoli (es. Admin/Logistica vs Autista).
* Dashboard Operativa con KPI (mezzi disponibili, in transito, in manutenzione, alert scadenze).

---

## Piano Operativo a Step (Esecuzione Sequenziale)

* **Step 1:** Analisi e inizializzazione della soluzione su VS 18 (struttura progetti, configurazione DI, predisposizione DbContext per SQLite e MariaDB). Attendi conferma.
* **Step 2:** Definizione dei Data Model EF Core, relazioni e migrazione iniziale. Attendi conferma.
* **Step 3:** Implementazione della logica di business e Data Access Services (inclusa la logica di calcolo automatico dei km e manutenzione). Attendi conferma.
* **Step 4:** Setup del Layout Blazor (responsive/mobile-ready) e della Dashboard Operativa. Attendi conferma.
* **Step 5:** Sviluppo moduli CRUD per le anagrafiche (Autisti, Mezzi, Clienti, Destinazioni). Attendi conferma.
* **Step 6:** Sviluppo modulo Tratte e interfaccia di tracciamento tappe in tempo reale per l'autista. Attendi conferma.
* **Step 7:** Sviluppo vista Storico Mezzo e sistema di alert manutenzioni. Attendi conferma.

---

## Istruzione di Inizio

Conferma di aver compreso queste istruzioni e forniscimi un'analisi dettagliata su come intendi strutturare la soluzione per lo **Step 1**. Poi fermati e attendi il mio via libera.