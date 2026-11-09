using FunEvents.Domain.Entities;
using Xunit;

namespace FunEvents.Tests.Domain
{
    public class EventoTests
    {
        [Fact]
        public void ReservaDescuentaStockYSubeVersion()
        {
            var show = new Evento(Guid.NewGuid(), "Show", DateTime.UtcNow.AddDays(10), 100);
            var v0 = show.Version;

            show.ReserveTickets(10);

            Assert.Equal(90, show.AvailableTickets);
            Assert.Equal(v0 + 1, show.Version);
        }

        [Fact]
        public void NoPermiteReservarEventoPasado()
        {
            var viejo = new Evento(Guid.NewGuid(), "Pasado", DateTime.UtcNow.AddDays(-1), 100);
            Assert.Throws<InvalidOperationException>(() => viejo.ReserveTickets(1));
        }

        [Fact]
        public void NoPermiteReservarMasDeLoDisponible()
        {
            var chico = new Evento(Guid.NewGuid(), "Chico", DateTime.UtcNow.AddDays(10), 5);
            Assert.Throws<InvalidOperationException>(() => chico.ReserveTickets(6));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-3)]
        public void ReservaRechazaCantidadInvalida(int qty)
        {
            var show = new Evento(Guid.NewGuid(), "Show", DateTime.UtcNow.AddDays(10), 100);
            Assert.Throws<ArgumentException>(() => show.ReserveTickets(qty));
        }

        [Fact]
        public void ReleaseDevuelveStockYRechazaCantidadInvalida()
        {
            var show = new Evento(Guid.NewGuid(), "Show", DateTime.UtcNow.AddDays(10), 100);
            show.ReleaseTickets(10);
            Assert.Equal(110, show.AvailableTickets);
            Assert.Throws<ArgumentException>(() => show.ReleaseTickets(0));
        }

        [Fact]
        public void ConstructorNormalizaTituloYRechazaInvalidos()
        {
            var show = new Evento(Guid.NewGuid(), "  Espacios  ", DateTime.UtcNow.AddDays(10), 10);
            Assert.Equal("Espacios", show.Title);

            Assert.Throws<ArgumentException>(() => new Evento(Guid.NewGuid(), "  ", DateTime.UtcNow.AddDays(1), 10));
            Assert.Throws<ArgumentException>(() => new Evento(Guid.NewGuid(), "Ok", DateTime.UtcNow.AddDays(1), -1));
        }

        [Fact]
        public void UpdateTrimeaYSubeVersion()
        {
            var show = new Evento(Guid.NewGuid(), "Show", DateTime.UtcNow.AddDays(10), 100);
            var v0 = show.Version;

            show.Update("  Nuevo  ", DateTime.UtcNow.AddDays(20), 50);

            Assert.Equal("Nuevo", show.Title);
            Assert.Equal(50, show.AvailableTickets);
            Assert.Equal(v0 + 1, show.Version);
        }
    }
}
