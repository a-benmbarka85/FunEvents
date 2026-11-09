using FunEvents.Domain.Entities;

namespace FunEvents.Application.Common.Dtos
{
    public sealed record ReservaDto(
        Guid Id,
        Guid EventoId,
        Guid UsuarioId,
        Guid? ColaboradorId,
        Guid TipoEntradaId,
        CanalVenta Canal,
        int TicketQuantity,
        DateTime ReservedAt)
    {
        public static ReservaDto From(Reserva reserva) =>
            new(
                reserva.Id,
                reserva.EventoId,
                reserva.UsuarioId,
                reserva.ColaboradorId,
                reserva.TipoEntradaId,
                reserva.Canal,
                reserva.TicketQuantity,
                reserva.ReservedAt);
    }

    public sealed record UpdateReservaRequest(
        Guid TipoEntradaId,
        int TicketQuantity,
        CanalVenta Canal,
        Guid? ColaboradorId);
}