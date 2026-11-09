using FunEvents.Domain.Entities;

namespace FunEvents.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(CancellationToken ct = default);
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(T entity, CancellationToken ct = default);
        Task UpdateAsync(T entity, CancellationToken ct = default);
        Task DeleteAsync(T entity, CancellationToken ct = default);
    }

    public interface ICategoriaRepository : IRepository<Categoria> { }
    public interface IColaboradorRepository : IRepository<Colaborador> { }
    public interface IEventRepository : IRepository<Evento> { }
    public interface IPagoRepository : IRepository<Pago> { }
    public interface IReservationRepository : IRepository<Reserva> { }
    public interface ITipoEntradaRepository : IRepository<TipoEntrada> { }
    public interface IUsuarioRepository : IRepository<Usuario> { }
}