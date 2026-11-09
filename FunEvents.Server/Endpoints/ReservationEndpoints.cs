using FunEvents.Application.Reservations.Commands;

namespace FunEvents.Server.Endpoints
{
    public static class ReservationEndpoints
    {
        public static void MapReservationEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/reservations", async (
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
            .Produces(StatusCodes.Status409Conflict).WithTags("Reservas");
        }
    }
}