using FunEvents.Application.Common.Interfaces;
using FunEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Server.Data.Repositories
{
    public sealed class TipoEntradaRepository(FunEventsDbContext context) : ITipoEntradaRepository
    {
        public Task<List<TipoEntrada>> GetAllAsync(CancellationToken cancellationToken = default) =>
            context.TiposEntrada.AsNoTracking().ToListAsync(cancellationToken);

        public Task<TipoEntrada?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.TiposEntrada.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        public async Task AddAsync(TipoEntrada tipoEntrada, CancellationToken cancellationToken = default) =>
            await context.TiposEntrada.AddAsync(tipoEntrada, cancellationToken);

        public Task UpdateAsync(TipoEntrada tipoEntrada, CancellationToken cancellationToken = default)
        {
            context.TiposEntrada.Update(tipoEntrada);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TipoEntrada tipoEntrada, CancellationToken cancellationToken = default)
        {
            context.TiposEntrada.Remove(tipoEntrada);
            return Task.CompletedTask;
        }
    }
}
