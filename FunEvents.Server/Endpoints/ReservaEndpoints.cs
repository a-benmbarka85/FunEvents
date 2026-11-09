using FunEvents.Application.Common.Dtos;
using FunEvents.Application.Common.Interfaces;
using FunEvents.Application.Reservations.Commands;

namespace FunEvents.Server.Endpoints
{
    public static class ReservaEndpoints
    {
        public static void MapReservaEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/reservas", async (IReservationRepository repository, CancellationToken ct) =>
            {
                var reservas = await repository.GetAllAsync(ct);
                return Results.Ok(reservas.Select(r => new ReservaDto(r.Id, r.EventoId, r.UsuarioId, r.ColaboradorId, r.TipoEntradaId, r.Canal, r.TicketQuantity, r.ReservedAt)));
            })
            .WithTags("Reservas");

            app.MapGet("/api/reservas/{id}", async (Guid id, IReservationRepository repository, CancellationToken ct) =>
            {
                var reserva = await repository.GetByIdAsync(id, ct);
                return reserva is null ? Results.NotFound() : Results.Ok(new ReservaDto(reserva.Id, reserva.EventoId, reserva.UsuarioId, reserva.ColaboradorId, reserva.TipoEntradaId, reserva.Canal, reserva.TicketQuantity, reserva.ReservedAt));
            })
            .WithTags("Reservas");

            app.MapPost("/api/reservas", async (
                CreateReservationCommand command,
                CreateReservationUseCase useCase,
                CancellationToken ct) =>
            {
                var response = await useCase.ExecuteAsync(command, ct);
                return Results.Created($"/api/reservas/{response.ReservaId}", response);
            })
            .Produces<ReservationResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
        .WithTags("Reservas");

            app.MapPut("/api/reservas/{id}", async (
                Guid id,
                UpdateReservaRequest request,
                UpdateReservationUseCase useCase,
                CancellationToken ct) =>
            {
                var command = new UpdateReservationCommand(
                    id,
                    request.TipoEntradaId,
                    request.TicketQuantity,
                    request.Canal,
                    request.ColaboradorId);

                var reserva = await useCase.ExecuteAsync(command, ct);
                return Results.Ok(reserva);
            })
            .RequireAuthorization()
            .WithTags("Reservas");

            app.MapDelete("/api/reservas/{id}", async (
                Guid id,
                DeleteReservationUseCase useCase,
                CancellationToken ct) =>
            {
                await useCase.ExecuteAsync(id, ct);
                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithTags("Reservas");
        }
    }
}