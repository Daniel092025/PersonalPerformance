# Warcraft Logs Performance Tracker

En C# konsoll applikasjon som henter og viser World of Warcraft raid ytelses data fra Warcraft Logs API

## Projekt

Prosjektet demonstrer API integrasjon i C# med:
- OAuth2 authentication
- GraphQL API consumption
- Ren arkitektur
- Async/await
- Konfigurasjonsstyring
- Data transformation layers

## Models

### Configuration
**Purpose**: Lagre applikasjon settings og API credentials.
- **`WarcraftLogsConfig`**: Holde API client ID, secret og base URL
  - Leser fra `appsettings.json`
  - Injecter inn i  services som trenger API tilgang.

### Domain Models (Models/Domain/)
**Purpose**: Representerer data

- **`PlayerPerformance`**
  - Inneholder: Character name, server, og liste over nylige rapporter.
  - Dette er hva appen jobber med internt.

- **`ReportSummary`**
  - Inneholder report code, title, start time

- **`FightPerformance`**
  - Boss name, DPS, kill status, duration

  ### API Models (Models/Api/)
