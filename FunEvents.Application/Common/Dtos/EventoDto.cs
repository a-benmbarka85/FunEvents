namespace FunEvents.Application.Common.Dtos
{
    public sealed record EventoDto(Guid Id, string Title, DateTime Date, int AvailableTickets);
    public sealed record CreateEventoRequest(string Title, DateTime Date, int AvailableTickets);
    public sealed record UpdateEventoRequest(string Title, DateTime Date, int AvailableTickets);
}
