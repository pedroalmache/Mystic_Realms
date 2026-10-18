# Mystic Realms

Mystic Realms is a fantasy dungeon-crawler card combat game. You build a party, delve through rooms, fight enemies in turn-based encounters, and pick rewards as you progress through a run.

## Who it’s for

- **Players** who enjoy deckbuilding combat and room-by-room dungeon runs (single-player today, with multiplayer UI hooks planned).
- **Developers** working on the game client, server, or combat systems in this monorepo.

## What’s in the repo

| Path | Description |
|------|-------------|
| [`apps/web`](apps/web) | Angular web client |
| [`apps/server`](apps/server) | ASP.NET Core API (Clean Architecture) |

The server is split into Domain, Application, Infrastructure, and ServerApi projects under `apps/server/src`.

## Built with

- **Frontend:** Angular 21, TypeScript, RxJS
- **Backend:** .NET 10, ASP.NET Core, MediatR, JWT auth (ASP.NET Identity)
- **Data:** Entity Framework Core + SQL Server
- **API docs:** Swagger / Swashbuckle (enabled in development)

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (LTS recommended) and npm
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB, Express, or full) reachable with the connection string below
- Optional: [`dotnet-ef`](https://learn.microsoft.com/ef/core/cli/dotnet) for migrations (`dotnet tool install --global dotnet-ef`)

## Configuration

Default API settings live in [`apps/server/src/MysticRealms.ServerApi/appsettings.json`](apps/server/src/MysticRealms.ServerApi/appsettings.json):

- **SQL Server:** `ConnectionStrings:DefaultConnection`  
  Default: `Server=localhost;Database=MysticRealms;Trusted_Connection=True;TrustServerCertificate=True;`
- **CORS:** allows `http://localhost:4200` (Angular dev server)
- **JWT:** configured under `JwtSettings` (change the secret before any real deployment)

The Angular client points at the API via [`apps/web/src/environments/environment.ts`](apps/web/src/environments/environment.ts) (`serverUrl: 'https://localhost:7100'`).

## Database setup

From `apps/server`:

```bash
dotnet ef database update ^
  -p src/MysticRealms.Infrastructure/MysticRealms.Infrastructure.csproj ^
  -s src/MysticRealms.ServerApi/MysticRealms.ServerApi.csproj
```

On macOS/Linux, use `\` instead of `^` for line continuations.

On first API startup, seed data runs automatically (including a local test user if none exists yet).

## How to run

Run the API and the web client in two terminals.

### 1. API

```bash
cd apps/server
dotnet run --project src/MysticRealms.ServerApi/MysticRealms.ServerApi.csproj
```

- HTTPS: `https://localhost:7100`
- HTTP: `http://localhost:5195`
- Swagger UI: `https://localhost:7100/swagger`

### 2. Web client

```bash
cd apps/web
npm install
npm start
```

Open `http://localhost:4200/`.

Sign up, or log in with an existing account, then start or continue a single-player run from the home screen.

## Useful commands

**Create a new EF migration** (from `apps/server`):

```bash
dotnet ef migrations add <MigrationName> ^
  -p src/MysticRealms.Infrastructure/MysticRealms.Infrastructure.csproj ^
  -s src/MysticRealms.ServerApi/MysticRealms.ServerApi.csproj
```

**Web unit tests:**

```bash
cd apps/web
npm test
```

**Web production build:**

```bash
cd apps/web
npm run build
```
