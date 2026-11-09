using FunEvents.Domain.Entities;

namespace FunEvents.Application.Common.Dtos
{
    public sealed record TipoEntradaDto(Guid Id, Guid EventoId, string Nombre, decimal Precio, string? Descripcion);
    public sealed record CreateTipoEntradaRequest(Guid EventoId, string Nombre, decimal Precio, string? Descripcion);
    public sealed record UpdateTipoEntradaRequest(string Nombre, decimal Precio, string? Descripcion);
}
