# FunEvents

Proyecto para manejar eventos y reservas de entradas (.NET 10 + PostgreSQL + frontend React). Lo fui armando por capas (Domain/Application/Server) para mantener las reglas de negocio separadas de la API y EF.

## qué incluye
- catálogos: eventos, tipos de entrada, usuarios, colaboradores, categorías, pagos
- reservas con canales: `Online`, `Oficina`, `Colaborador` (si es Colaborador, exige `colaboradorId`)
- API con Minimal APIs + Swagger, seed de datos, cliente de consola

## estructura (resumen)
- `FunEvents.Domain` — entidades/reglas
- `FunEvents.Application` — casos de uso, DTOs, interfaces (`IRepository<T>`, `IUnitOfWork`)
- `FunEvents.Server` — API, EF Core/Npgsql, repos, auth por API key (prototipo), migraciones/seed
- `FunEvents.AppHost` — Aspire
- `FunEvents.ConsoleClient` — pruebas rápidas
- `frontend/` — React + Vite
- `FunEvents.Tests` — xUnit + Moq

## decisiones rápidas
- Minimal APIs, sin MediatR (por ahora no aporta).
- `IRepository<T>` genérico para no repetir CRUD.

## flujo de reserva (importante)
`CreateReservationUseCase` busca evento/usuario/tipo, valida que el tipo pertenezca al evento, aplica `Evento.ReserveTickets`, crea `Reserva` (con regla de canal) y guarda cambios.  

## cómo correr
**Aspire:** `dotnet run --project FunEvents.AppHost` (ver URL en consola)  
**Docker Compose (raíz):** `docker compose up -d --build` (defaults aprox: API `8087`, PG host `5439`, pgAdmin `5057`)  
**Server solo:** `dotnet run --project FunEvents.Server` (necesita `postgresdb`)  
**Frontend:** `cd frontend && npm install && npm run dev`

## seed
`POST /api/seed` con `X-Api-Key`.

## tests
```bash
dotnet test FunEvents.Tests/FunEvents.Tests.csproj
```