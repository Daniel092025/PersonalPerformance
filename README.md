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
**Lagre applikasjon settings og API credentials**: 
- **`WarcraftLogsConfig`**: Holde API client ID, secret og base URL
  - Leser fra `appsettings.json`
  - Injecter inn i  services som trenger API tilgang.

### Domain Models (Models/Domain/)
**Representerer data**:

- **`PlayerPerformance`**
  - Inneholder: Character name, server, og liste over nylige rapporter.
  - Dette er hva appen jobber med internt.

- **`ReportSummary`**
  - Inneholder report code, title, start time

- **`FightPerformance`**
  - Boss name, DPS, kill status, duration

  ### API Models (Models/Api/)
  **Matche Data strukturen fra WarcraftLogs**:

  - **`GraphQLResponse`**:
  - Top-level container for alle GraphQL svar
  - Inneholder `CharacterDataResponse` (CharacterData)

  - **`CharacterDataResponse`**:
  - Direkte mapping til GraphQL's `characterData` felt

  - **`CharacterResponse`**:
  - Name, server info, nylige rapporter
  - Matcher API field navn nøyaktig (hence `[JsonPropertyName]` attributter)

  - **`ServerResponse`**:
  - Simpel container for server navn

  - **`RecentReportsResponse`**:
  - Inneholder array av `ReportData`

  - **`ReportData`**:
  - Rapport kode, title, timestamp, fights
  - Kan inneholde felter, som ikke blir brukt enda.

  - **`FightData`**:
  - ID, name, kill status (nullable!), timestamps
  - Direkte API mapping med alle "særheter" (Rare boss navn, osv osv.)

  - **`TokenResponse`**:    
  - Access token, token type, utløpsdato
  - Brukes under authentication


## Services 

### `IWarcraftLogsService` (Interface)
Definerer kontrakten for interakjson med Warcraft Logs API.

**Methods**:
- `GetAccessTokenAsync()`: Får OAuth2 access token
- `GetPlayerPerformanceAsync()`: Henter character performance data


### `WarcraftLogsService` (Implementation)
Håndterer alt av API kommunikasjon og data transformasjon.


**Key Methods**:
- `GetAccessTokenAsync()`: Implementerer OAuth2 authentication
- `GetGraphQLClientAsync()`: Lager authenticated GraphQL client
- `GetPlayerPerformanceAsync()`: Hovedmetoden - henter og tranformerer data
- `MapToPlayerPerformance()`: Transforms API DTOs til domain modeller (models)


##  Setup

### Krav til bruk
- .NET 8.0 SDK or later
- Warcraft Logs API account

### 1. API Credentials
1. Visit https://www.warcraftlogs.com/api/clients/
2. Create a new client
3. Save your Client ID and Client Secret

### 2. Konfigure Applikasjonen
Create/update `appsettings.json`:
```json
{
  "WarcraftLogs": {
    "ClientId": "your_client_id_here",
    "ClientSecret": "your_client_secret_here",
    "ApiBaseUrl": "https://www.warcraftlogs.com/api/v2"
  }
}
```

### 3. Installere Dependencies
```bash
dotnet restore
```

### 4. Kjøre
```bash
dotnet run
``` 

