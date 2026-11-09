namespace FunEvents.Application.Common.Dtos
{
    public sealed record ColaboradorDto(Guid Id, string Name, string Email);
    public sealed record CreateColaboradorRequest(string Name, string Email);
    public sealed record UpdateColaboradorRequest(string Name, string Email);
}
