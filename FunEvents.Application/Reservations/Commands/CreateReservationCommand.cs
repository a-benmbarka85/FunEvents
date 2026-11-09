using FunEvents.Domain.Entities;

namespace FunEvents.Application.Reservations.Commands
{
    public sealed record CreateReservationCommand(Guid EventoId, Guid UsuarioId, Guid TipoEntradaId, int TicketQuantity, CanalVenta Canal = CanalVenta.Online, Guid? ColaboradorId = null);
    public sealed record ReservationResponse(Guid ReservaId, Guid EventoId, Guid UsuarioId, Guid TipoEntradaId, int QuantityReserved);
}
