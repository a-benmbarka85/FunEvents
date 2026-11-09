using FunEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Server.Data
{
    public sealed class FunEventsDbContext(DbContextOptions<FunEventsDbContext> options) : DbContext(options)
    {
        public DbSet<Evento> Eventos => Set<Evento>();
        public DbSet<Colaborador> Colaboradores => Set<Colaborador>();
        public DbSet<Reserva> Reservas => Set<Reserva>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Pago> Pagos => Set<Pago>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<TipoEntrada> TiposEntrada => Set<TipoEntrada>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FunEventsDbContext).Assembly);
        }
    }
}
