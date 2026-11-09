using FunEvents.Application.Common.Interfaces;
using FunEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Server.Data.Repositories
{
    public sealed class EventRepository(FunEventsDbContext context) : IEventRepository
    {
        public Task<List<Evento>> GetAllAsync(CancellationToken cancellationToken = default) =>
            context.Eventos.AsNoTracking().ToListAsync(cancellationToken);

        public Task<Evento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Eventos.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        public async Task AddAsync(Evento evento, CancellationToken cancellationToken = default) =>
            await context.Eventos.AddAsync(evento, cancellationToken);

        public Task UpdateAsync(Evento evento, CancellationToken cancellationToken = default)
        {
            context.Eventos.Update(evento);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Evento evento, CancellationToken cancellationToken = default)
        {
            context.Eventos.Remove(evento);
            return Task.CompletedTask;
        }
    }
}
