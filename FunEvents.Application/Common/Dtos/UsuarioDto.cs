namespace FunEvents.Application.Common.Dtos
{
    public sealed record UsuarioDto(Guid Id, string Nombre, string Email, string Rol, DateTime FechaRegistro);

    public sealed record CreateUsuarioRequest(string Nombre, string Email, string Rol);

    public sealed record UpdateUsuarioRequest(string Nombre, string Email, string Rol);
}
