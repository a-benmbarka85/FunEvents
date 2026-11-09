using FunEvents.Application.Common.Dtos;
using FunEvents.Application.Common.Interfaces;
using FunEvents.Domain.Entities;

namespace FunEvents.Server.Endpoints
{
    public static class EventoEndpoints
    {
        public static void MapEventoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/eventos", async (IEventRepository repository, CancellationToken ct) =>
            {
                var eventos = await repository.GetAllAsync(ct);
                return Results.Ok(eventos.Select(e => new EventoDto(e.Id, e.Title, e.Date, e.AvailableTickets)));
            })
            .WithTags("Eventos");

            app.MapGet("/api/eventos/{id}", async (Guid id, IEventRepository repository, CancellationToken ct) =>
            {
                var evento = await repository.GetByIdAsync(id, ct);
                return evento is null ? Results.NotFound() : Results.Ok(new EventoDto(evento.Id, evento.Title, evento.Date, evento.AvailableTickets));
            })
            .WithTags("Eventos");

            app.MapPost("/api/eventos", async (
                CreateEventoRequest request,
                IEventRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var evento = new Evento(Guid.NewGuid(), request.Title, request.Date, request.AvailableTickets);
                await repository.AddAsync(evento, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.Created($"/api/eventos/{evento.Id}", new EventoDto(evento.Id, evento.Title, evento.Date, evento.AvailableTickets));
            })
            .RequireAuthorization()
            .WithTags("Eventos");

            app.MapPut("/api/eventos/{id}", async (
                Guid id,
                UpdateEventoRequest request,
                IEventRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var evento = await repository.GetByIdAsync(id, ct);
                if (evento is null) return Results.NotFound();

                evento.Update(request.Title, request.Date, request.AvailableTickets);
                await repository.UpdateAsync(evento, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.Ok(new EventoDto(evento.Id, evento.Title, evento.Date, evento.AvailableTickets));
            })
            .RequireAuthorization()
            .WithTags("Eventos");

            app.MapDelete("/api/eventos/{id}", async (
                Guid id,
                IEventRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var evento = await repository.GetByIdAsync(id, ct);
                if (evento is null) return Results.NotFound();

                await repository.DeleteAsync(evento, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithTags("Eventos");
        }
    }
}