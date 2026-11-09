using FunEvents.Application.Common.Dtos;
using FunEvents.Application.Common.Interfaces;
using FunEvents.Domain.Entities;

namespace FunEvents.Server.Endpoints
{
    public static class PagoEndpoints
    {
        public static void MapPagoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/pagos", async (IPagoRepository repository, CancellationToken ct) =>
            {
                var pagos = await repository.GetAllAsync(ct);
                return Results.Ok(pagos.Select(p => new PagoDto(p.Id, p.ReservaId, p.Monto, p.MetodoPago, p.Estado, p.FechaPago)));
            })
            .WithTags("Pagos");

            app.MapGet("/api/pagos/{id}", async (Guid id, IPagoRepository repository, CancellationToken ct) =>
            {
                var pago = await repository.GetByIdAsync(id, ct);
                return pago is null ? Results.NotFound() : Results.Ok(new PagoDto(pago.Id, pago.ReservaId, pago.Monto, pago.MetodoPago, pago.Estado, pago.FechaPago));
            })
            .WithTags("Pagos");

            app.MapPost("/api/pagos", async (
                CreatePagoRequest request,
                IPagoRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var pago = new Pago(
                    Guid.NewGuid(),
                    request.ReservaId,
                    request.Monto,
                    request.MetodoPago,
                    request.Estado,
                    request.FechaPago);

                await repository.AddAsync(pago, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.Created($"/api/pagos/{pago.Id}", new PagoDto(pago.Id, pago.ReservaId, pago.Monto, pago.MetodoPago, pago.Estado, pago.FechaPago));
            })
            .RequireAuthorization().WithTags("Pagos");

            app.MapPut("/api/pagos/{id}", async (
                Guid id,
                UpdatePagoRequest request,
                IPagoRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var pago = await repository.GetByIdAsync(id, ct);
                if (pago is null) return Results.NotFound();

                pago.Update(request.Monto, request.MetodoPago, request.Estado, request.FechaPago);
                await repository.UpdateAsync(pago, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.Ok(new PagoDto(pago.Id, pago.ReservaId, pago.Monto, pago.MetodoPago, pago.Estado, pago.FechaPago));
            })
            .RequireAuthorization()
            .WithTags("Pagos");

            app.MapDelete("/api/pagos/{id}", async (
                Guid id,
                IPagoRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var pago = await repository.GetByIdAsync(id, ct);
                if (pago is null) return Results.NotFound();

                await repository.DeleteAsync(pago, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithTags("Pagos");
        }
    }
}