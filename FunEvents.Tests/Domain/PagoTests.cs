using FunEvents.Domain.Entities;
using Xunit;

namespace FunEvents.Tests.Domain
{
    public sealed class PagoTests
    {
        [Fact]
        public void CreaYNormalizaStrings()
        {
            var pago = new Pago(Guid.NewGuid(), Guid.NewGuid(), 150m, "  Tarjeta  ", "  Pagado  ", DateTime.UtcNow);

            Assert.Equal("Tarjeta", pago.MetodoPago);
            Assert.Equal("Pagado", pago.Estado);
        }

        [Fact]
        public void RechazaDatosInvalidosEnCreacion()
        {
            var rid = Guid.NewGuid();
            var now = DateTime.UtcNow;

            Assert.Throws<ArgumentException>(() => new Pago(Guid.NewGuid(), Guid.Empty, 10m, "Tarjeta", "Pagado", now));
            Assert.Throws<ArgumentException>(() => new Pago(Guid.NewGuid(), rid, -1m, "Tarjeta", "Pagado", now));
            Assert.Throws<ArgumentException>(() => new Pago(Guid.NewGuid(), rid, 10m, "  ", "Pagado", now));
            Assert.Throws<ArgumentException>(() => new Pago(Guid.NewGuid(), rid, 10m, "Tarjeta", "", now));
        }

        [Fact]
        public void AceptaMontoCero()
        {
            var pago = new Pago(Guid.NewGuid(), Guid.NewGuid(), 0m, "Cortesia", "Pagado", DateTime.UtcNow);
            Assert.Equal(0m, pago.Monto);
        }

        // FIXME: Update no valida monto/strings (a diferencia del ctor) — inconsistencia de dominio a revisar.
    }
}
