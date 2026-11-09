using FunEvents.Application.Common.Interfaces;
using FunEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Server.Data.Repositories
{
    public sealed class ReservationRepository(FunEventsDbContext context) : IReservationRepository
    {
        public Task<List<Reserva>> GetAllAsync(CancellationToken cancellationToken = default) =>
            context.Reservas.AsNoTracking().ToListAsync(cancellationToken);

        public Task<Reserva?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Reservas.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        public async Task AddAsync(Reserva reserva, CancellationToken cancellationToken = default) =>
            await context.Reservas.AddAsync(reserva, cancellationToken);

        public Task UpdateAsync(Reserva reserva, CancellationToken cancellationToken = default)
        {
            context.Reservas.Update(reserva);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Reserva reserva, CancellationToken cancellationToken = default)
        {
            context.Reservas.Remove(reserva);
            return Task.CompletedTask;
        }
    }
}
