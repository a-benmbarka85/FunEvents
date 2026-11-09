using FunEvents.Application.Common.Interfaces;
using FunEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Server.Data.Repositories
{
    public sealed class UsuarioRepository(FunEventsDbContext context) : IUsuarioRepository
    {
        public Task<List<Usuario>> GetAllAsync(CancellationToken cancellationToken = default) =>
            context.Usuarios.AsNoTracking().ToListAsync(cancellationToken);

        public Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Usuarios.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        public async Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default) =>
            await context.Usuarios.AddAsync(usuario, cancellationToken);

        public Task UpdateAsync(Usuario usuario, CancellationToken cancellationToken = default)
        {
            context.Usuarios.Update(usuario);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Usuario usuario, CancellationToken cancellationToken = default)
        {
            context.Usuarios.Remove(usuario);
            return Task.CompletedTask;
        }
    }
}
