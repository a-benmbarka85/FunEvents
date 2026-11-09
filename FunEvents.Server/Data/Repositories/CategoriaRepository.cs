using FunEvents.Application.Common.Interfaces;
using FunEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Server.Data.Repositories
{
    public sealed class CategoriaRepository(FunEventsDbContext context) : ICategoriaRepository
    {
        public Task<List<Categoria>> GetAllAsync(CancellationToken cancellationToken = default) =>
            context.Categorias.AsNoTracking().ToListAsync(cancellationToken);

        public Task<Categoria?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Categorias.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task AddAsync(Categoria categoria, CancellationToken cancellationToken = default) =>
            await context.Categorias.AddAsync(categoria, cancellationToken);

        public Task UpdateAsync(Categoria categoria, CancellationToken cancellationToken = default)
        {
            context.Categorias.Update(categoria);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Categoria categoria, CancellationToken cancellationToken = default)
        {
            context.Categorias.Remove(categoria);
            return Task.CompletedTask;
        }
    }
}
