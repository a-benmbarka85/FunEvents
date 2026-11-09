using FunEvents.Application.Common.Exceptions;
using FunEvents.Application.Common.Interfaces;
using FunEvents.Domain.Entities;

namespace FunEvents.Application.Reservations.Commands
{
    public sealed class CreateReservationUseCase(
        IEventRepository eventRepository,
        ITipoEntradaRepository tipoEntradaRepository,
        IUsuarioRepository usuarioRepository,
        IReservationRepository reservationRepository,
        IUnitOfWork unitOfWork)
    {
        public async Task<ReservationResponse> ExecuteAsync(CreateReservationCommand command, CancellationToken cancellationToken = default)
        {
            var evento = await eventRepository.GetByIdAsync(command.EventoId, cancellationToken)
                ?? throw new NotFoundException("Evento no encontrado.");

            var usuario = await usuarioRepository.GetByIdAsync(command.UsuarioId, cancellationToken)
                ?? throw new NotFoundException("Usuario no encontrado.");

            var tipoEntrada = await tipoEntradaRepository.GetByIdAsync(command.TipoEntradaId, cancellationToken)
                ?? throw new NotFoundException("Tipo de entrada no encontrado.");

            if (!tipoEntrada.BelongsTo(evento.Id))
                throw new ArgumentException("El tipo de entrada no pertenece a este evento.");

            evento.ReserveTickets(command.TicketQuantity);

            var reserva = new Reserva(
                Guid.NewGuid(),
                evento.Id,
                usuario.Id,
                tipoEntrada.Id,
                command.TicketQuantity,
                command.Canal,
                command.ColaboradorId);

            await reservationRepository.AddAsync(reserva, cancellationToken);
            await eventRepository.UpdateAsync(evento, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new ReservationResponse(reserva.Id, evento.Id, usuario.Id, tipoEntrada.Id, reserva.TicketQuantity);
        }
    }
}