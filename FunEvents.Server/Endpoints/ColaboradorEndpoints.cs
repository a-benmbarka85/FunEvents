using FunEvents.Application.Common.Dtos;
using FunEvents.Application.Common.Interfaces;
using FunEvents.Domain.Entities;

namespace FunEvents.Server.Endpoints
{
    public static class ColaboradorEndpoints
    {
        public static void MapColaboradorEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/colaboradores", async (IColaboradorRepository repository, CancellationToken ct) =>
            {
                var colaboradores = await repository.GetAllAsync(ct);
                return Results.Ok(colaboradores.Select(c => new ColaboradorDto(c.Id, c.Name, c.Email)));
            })
            .WithTags("Colaboradores");

            app.MapGet("/api/colaboradores/{id}", async (Guid id, IColaboradorRepository repository, CancellationToken ct) =>
            {
                var colaborador = await repository.GetByIdAsync(id, ct);
                return colaborador is null ? Results.NotFound() : Results.Ok(new ColaboradorDto(colaborador.Id, colaborador.Name, colaborador.Email));
            })
            .WithTags("Colaboradores");

            app.MapPost("/api/colaboradores", async (
                CreateColaboradorRequest request,
                IColaboradorRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var colaborador = new Colaborador(Guid.NewGuid(), request.Name, request.Email);
                await repository.AddAsync(colaborador, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.Created($"/api/colaboradores/{colaborador.Id}", new ColaboradorDto(colaborador.Id, colaborador.Name, colaborador.Email));
            })
            .RequireAuthorization()
            .WithTags("Colaboradores");

            app.MapPut("/api/colaboradores/{id}", async (
                Guid id,
                UpdateColaboradorRequest request,
                IColaboradorRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var colaborador = await repository.GetByIdAsync(id, ct);
                if (colaborador is null) return Results.NotFound();

                colaborador.Update(request.Name, request.Email);
                await repository.UpdateAsync(colaborador, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.Ok(new ColaboradorDto(colaborador.Id, colaborador.Name, colaborador.Email));
            })
            .RequireAuthorization()
            .WithTags("Colaboradores");

            app.MapDelete("/api/colaboradores/{id}", async (
                Guid id,
                IColaboradorRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var colaborador = await repository.GetByIdAsync(id, ct);
                if (colaborador is null) return Results.NotFound();

                await repository.DeleteAsync(colaborador, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithTags("Colaboradores");
        }
    }
}