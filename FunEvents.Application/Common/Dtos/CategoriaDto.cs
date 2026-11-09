namespace FunEvents.Application.Common.Dtos
{
    public sealed record CategoriaDto(Guid Id, string Nombre, string? Descripcion);
    public sealed record CreateCategoriaRequest(string Nombre, string? Descripcion);
    public sealed record UpdateCategoriaRequest(string Nombre, string? Descripcion);
}
