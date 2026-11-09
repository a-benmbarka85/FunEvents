using FunEvents.Application.Common.Exceptions;
using FunEvents.Application.Common.Interfaces;

namespace FunEvents.Application.Reservations.Commands
{
    public sealed class DeleteReservationUseCase(
        IReservationRepository reservationRepository,
        IEventRepository eventRepository,
        IUnitOfWork unitOfWork)
    {
        public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var reserva = await reservationRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException("Reserva no encontrada.");

            var evento = await eventRepository.GetByIdAsync(reserva.EventoId, cancellationToken)
                ?? throw new NotFoundException("Evento no encontrado.");

            // Al cancelar la reserva se devuelve la cantidad al total disponible.
            evento.ReleaseTickets(reserva.TicketQuantity);

            await reservationRepository.DeleteAsync(reserva, cancellationToken);
            await eventRepository.UpdateAsync(evento, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
