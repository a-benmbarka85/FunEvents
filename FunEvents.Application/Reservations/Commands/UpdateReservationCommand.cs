using FunEvents.Domain.Entities;

namespace FunEvents.Application.Reservations.Commands
{
    public sealed record UpdateReservationCommand(
        Guid ReservaId,
        Guid TipoEntradaId,
        int TicketQuantity,
        CanalVenta Canal = CanalVenta.Online,
        Guid? ColaboradorId = null);
}
