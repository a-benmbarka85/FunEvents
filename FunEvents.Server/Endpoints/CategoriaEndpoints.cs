using FunEvents.Application.Common.Dtos;
using FunEvents.Application.Common.Interfaces;
using FunEvents.Domain.Entities;

namespace FunEvents.Server.Endpoints
{
    public static class CategoriaEndpoints
    {
        public static void MapCategoriaEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/categorias", async (ICategoriaRepository repository, CancellationToken ct) =>
            {
                var categorias = await repository.GetAllAsync(ct);
                return Results.Ok(categorias.Select(c => new CategoriaDto(c.Id, c.Nombre, c.Descripcion)));
            })
            .WithTags("Categorias");

            app.MapGet("/api/categorias/{id}", async (Guid id, ICategoriaRepository repository, CancellationToken ct) =>
            {
                var categoria = await repository.GetByIdAsync(id, ct);
                return categoria is null ? Results.NotFound() : Results.Ok(new CategoriaDto(categoria.Id, categoria.Nombre, categoria.Descripcion));
            })
            .WithTags("Categorias");

            app.MapPost("/api/categorias", async (
                CreateCategoriaRequest request,
                ICategoriaRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var categoria = new Categoria(Guid.NewGuid(), request.Nombre, request.Descripcion);
                await repository.AddAsync(categoria, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.Created($"/api/categorias/{categoria.Id}", new CategoriaDto(categoria.Id, categoria.Nombre, categoria.Descripcion));
            })
            .RequireAuthorization()
            .WithTags("Categorias");

            app.MapPut("/api/categorias/{id}", async (
                Guid id,
                UpdateCategoriaRequest request,
                ICategoriaRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var categoria = await repository.GetByIdAsync(id, ct);
                if (categoria is null) return Results.NotFound();

                categoria.Update(request.Nombre, request.Descripcion);
                await repository.UpdateAsync(categoria, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.Ok(new CategoriaDto(categoria.Id, categoria.Nombre, categoria.Descripcion));
            })
            .RequireAuthorization()
            .WithTags("Categorias");

            app.MapDelete("/api/categorias/{id}", async (
                Guid id,
                ICategoriaRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var categoria = await repository.GetByIdAsync(id, ct);
                if (categoria is null) return Results.NotFound();

                await repository.DeleteAsync(categoria, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithTags("Categorias");
        }
    }
}