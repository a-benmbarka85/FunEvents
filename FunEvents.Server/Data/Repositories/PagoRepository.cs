using FunEvents.Application.Common.Interfaces;
using FunEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Server.Data.Repositories
{
    public sealed class PagoRepository(FunEventsDbContext context) : IPagoRepository
    {
        public Task<List<Pago>> GetAllAsync(CancellationToken cancellationToken = default) =>
            context.Pagos.AsNoTracking().ToListAsync(cancellationToken);

        public Task<Pago?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Pagos.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        public async Task AddAsync(Pago pago, CancellationToken cancellationToken = default) =>
            await context.Pagos.AddAsync(pago, cancellationToken);

        public Task UpdateAsync(Pago pago, CancellationToken cancellationToken = default)
        {
            context.Pagos.Update(pago);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Pago pago, CancellationToken cancellationToken = default)
        {
            context.Pagos.Remove(pago);
            return Task.CompletedTask;
        }
    }
}
