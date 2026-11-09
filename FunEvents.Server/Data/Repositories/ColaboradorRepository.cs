using FunEvents.Application.Common.Interfaces;
using FunEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Server.Data.Repositories
{
    public sealed class ColaboradorRepository(FunEventsDbContext context) : IColaboradorRepository
    {
        public Task<List<Colaborador>> GetAllAsync(CancellationToken cancellationToken = default) =>
            context.Colaboradores.AsNoTracking().ToListAsync(cancellationToken);

        public Task<Colaborador?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Colaboradores.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task AddAsync(Colaborador colaborador, CancellationToken cancellationToken = default) =>
            await context.Colaboradores.AddAsync(colaborador, cancellationToken);

        public Task UpdateAsync(Colaborador colaborador, CancellationToken cancellationToken = default)
        {
            context.Colaboradores.Update(colaborador);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Colaborador colaborador, CancellationToken cancellationToken = default)
        {
            context.Colaboradores.Remove(colaborador);
            return Task.CompletedTask;
        }
    }
}
