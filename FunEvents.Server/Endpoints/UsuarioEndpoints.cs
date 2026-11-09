using FunEvents.Application.Common.Dtos;
using FunEvents.Application.Common.Interfaces;
using FunEvents.Domain.Entities;

namespace FunEvents.Server.Endpoints
{
    public static class UsuarioEndpoints
    {
        public static void MapUsuarioEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/usuarios", async (IUsuarioRepository repository, CancellationToken ct) =>
            {
                var usuarios = await repository.GetAllAsync(ct);
                return Results.Ok(usuarios.Select(u => new UsuarioDto(u.Id, u.Nombre, u.Email, u.Rol, u.FechaRegistro)));
            }).WithTags("usuarios");

            app.MapGet("/api/usuarios/{id}", async (Guid id, IUsuarioRepository repository, CancellationToken ct) =>
            {
                var usuario = await repository.GetByIdAsync(id, ct);
                return usuario is null ? Results.NotFound() : Results.Ok(new UsuarioDto(usuario.Id, usuario.Nombre, usuario.Email, usuario.Rol, usuario.FechaRegistro));
            }).WithTags("usuarios");

            app.MapPost("/api/usuarios", async (
                CreateUsuarioRequest request,
                IUsuarioRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var usuario = new Usuario(Guid.NewGuid(), request.Nombre, request.Email, request.Rol);
                await repository.AddAsync(usuario, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.Created($"/api/usuarios/{usuario.Id}", new UsuarioDto(usuario.Id, usuario.Nombre, usuario.Email, usuario.Rol, usuario.FechaRegistro));
            })
            .RequireAuthorization()
            .WithTags("usuarios");

            app.MapPut("/api/usuarios/{id}", async (
                Guid id,
                UpdateUsuarioRequest request,
                IUsuarioRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var usuario = await repository.GetByIdAsync(id, ct);
                if (usuario is null) return Results.NotFound();

                usuario.Update(request.Nombre, request.Email, request.Rol);
                await repository.UpdateAsync(usuario, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.Ok(new UsuarioDto(usuario.Id, usuario.Nombre, usuario.Email, usuario.Rol, usuario.FechaRegistro));
            })
            .RequireAuthorization().WithTags("usuarios");

            app.MapDelete("/api/usuarios/{id}", async (
                Guid id,
                IUsuarioRepository repository,
                IUnitOfWork unitOfWork,
                CancellationToken ct) =>
            {
                var usuario = await repository.GetByIdAsync(id, ct);
                if (usuario is null) return Results.NotFound();

                await repository.DeleteAsync(usuario, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Results.NoContent();
            })
            .RequireAuthorization().WithTags("usuarios");
        }
    }
}