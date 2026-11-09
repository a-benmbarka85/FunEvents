using FunEvents.Application.Common.Exceptions;
using FunEvents.Application.Common.Interfaces;

namespace FunEvents.Application.Reservations.Commands
{
    public sealed class UpdateReservationUseCase(
        IReservationRepository reservationRepository,
        ITipoEntradaRepository tipoEntradaRepository,
        IEventRepository eventRepository,
        IUnitOfWork unitOfWork)
    {
        public async Task<ReservationResponse> ExecuteAsync(UpdateReservationCommand command, CancellationToken cancellationToken = default)
        {
            var reserva = await reservationRepository.GetByIdAsync(command.ReservaId, cancellationToken)
                ?? throw new NotFoundException("Reserva no encontrada.");

            var tipoEntrada = await tipoEntradaRepository.GetByIdAsync(command.TipoEntradaId, cancellationToken)
                ?? throw new NotFoundException("Tipo de entrada no encontrado.");

            var evento = await eventRepository.GetByIdAsync(reserva.EventoId, cancellationToken)
                ?? throw new NotFoundException("Evento no encontrado.");

            var cantidadAnterior = reserva.TicketQuantity;

            reserva.Update(command.TipoEntradaId, command.TicketQuantity, command.Canal, command.ColaboradorId);

            evento.ReleaseTickets(cantidadAnterior);
            evento.ReserveTickets(command.TicketQuantity);

            await reservationRepository.UpdateAsync(reserva, cancellationToken);
            await eventRepository.UpdateAsync(evento, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var eventoId = reserva.EventoId;
            return new ReservationResponse(reserva.Id, eventoId, reserva.UsuarioId, reserva.TipoEntradaId, reserva.TicketQuantity);
        }
    }
}
