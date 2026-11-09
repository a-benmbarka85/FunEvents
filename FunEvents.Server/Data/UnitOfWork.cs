using FunEvents.Application.Common.Interfaces;

namespace FunEvents.Server.Data
{
    public sealed class UnitOfWork(FunEventsDbContext context) : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            context.SaveChangesAsync(cancellationToken);
    }
}
