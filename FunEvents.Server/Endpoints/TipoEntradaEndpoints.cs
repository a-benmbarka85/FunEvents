using FunEvents.Application.Common.Dtos;
using FunEvents.Application.Common.Interfaces;
using FunEvents.Domain.Entities;

namespace FunEvents.Server.Endpoints
{
    public static class TipoEntradaEndpoints
    {
        public static void MapTipoEntradaEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/tipos-entrada", async (ITipoEntradaRepository repository, CancellationToken ct) =>
            {
                var tiposEntrada = await repository.GetAllAsync(ct);
                return Results.Ok(tiposEntrada.Select(t => new TipoEntradaDto(t.Id, t.EventoId, t.Nombre, t.Precio, t.Descripcion)));
            })
            .WithTags("TiposEntrada");

            app.MapGet("/api/tipos-entrada/{id}", async (Guid id, ITipoEntradaRepository repository, CancellationToken ct) =>
            {
                var tipoEntrada = await repository.GetByIdAsync(id, ct);
                return tipoEntrada is null ? Results.NotFound() : Results.Ok(new TipoEntradaDto(tipoEntrada.Id, tipoEntrada.EventoId, tipoEntrada.Nombre, tipoEntrada.Precio, tipoEntrada.Descripcion));
            })
            .WithTags("TiposEntrada");

            app.MapPost("/api/tipos-entrada", async (
                CreateTipoEntradaRequest request,
                ITipoEntradaRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var tipoEntrada = new TipoEntrada(
                    Guid.NewGuid(),
                    request.EventoId,
                    request.Nombre,
                    request.Precio,
                    request.Descripcion);

                await repository.AddAsync(tipoEntrada, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.Created($"/api/tipos-entrada/{tipoEntrada.Id}", new TipoEntradaDto(tipoEntrada.Id, tipoEntrada.EventoId, tipoEntrada.Nombre, tipoEntrada.Precio, tipoEntrada.Descripcion));
            })
            .RequireAuthorization()
            .WithTags("TiposEntrada");

            app.MapPut("/api/tipos-entrada/{id}", async (
                Guid id,
                UpdateTipoEntradaRequest request,
                ITipoEntradaRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var tipoEntrada = await repository.GetByIdAsync(id, ct);
                if (tipoEntrada is null) return Results.NotFound();

                tipoEntrada.Update(request.Nombre, request.Precio, request.Descripcion);
                await repository.UpdateAsync(tipoEntrada, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.Ok(new TipoEntradaDto(tipoEntrada.Id, tipoEntrada.EventoId, tipoEntrada.Nombre, tipoEntrada.Precio, tipoEntrada.Descripcion));
            })
            .RequireAuthorization()
            .WithTags("TiposEntrada");

            app.MapDelete("/api/tipos-entrada/{id}", async (
                Guid id,
                ITipoEntradaRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var tipoEntrada = await repository.GetByIdAsync(id, ct);
                if (tipoEntrada is null) return Results.NotFound();

                await repository.DeleteAsync(tipoEntrada, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithTags("TiposEntrada");
        }
    }
}