using FunEvents.Domain.Entities;
using Xunit;

namespace FunEvents.Tests.Domain
{
    public class ReservaTests
    {
        private static readonly Guid VipId = Guid.Parse("34197f02-aeba-476b-a5af-d8a54a96ee37");

        [Fact]
        public void CreaConDefaultsCorrectos()
        {
            var r = new Reserva(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), VipId, 2);

            Assert.Equal(CanalVenta.Online, r.Canal);
            Assert.Null(r.ColaboradorId);
            Assert.True(r.ReservedAt <= DateTime.UtcNow);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void RechazaCantidadInvalida(int qty)
        {
            Assert.Throws<ArgumentException>(() =>
                new Reserva(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), VipId, qty));
        }

        [Fact]
        public void RequiereIdsObligatorios()
        {
            Assert.Throws<ArgumentException>(() => new Reserva(Guid.NewGuid(), Guid.Empty, Guid.NewGuid(), VipId, 1));
            Assert.Throws<ArgumentException>(() => new Reserva(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty, VipId, 1));
            Assert.Throws<ArgumentException>(() => new Reserva(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.Empty, 1));
        }

        [Fact]
        public void CanalColaboradorExigeColaboradorId()
        {
            Assert.Throws<ArgumentException>(() =>
                new Reserva(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), VipId, 2, CanalVenta.Colaborador));

            var ok = new Reserva(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), VipId, 2, CanalVenta.Colaborador, Guid.NewGuid());
            Assert.NotNull(ok.ColaboradorId);
        }

        [Fact]
        public void UpdateCambiaEstadoYValidaReglas()
        {
            var r = new Reserva(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), VipId, 4);

            r.Update(VipId, 2, CanalVenta.Oficina, null);

            Assert.Equal(2, r.TicketQuantity);
            Assert.Equal(CanalVenta.Oficina, r.Canal);
            Assert.Throws<ArgumentException>(() => r.Update(VipId, 0, CanalVenta.Online, null));
            Assert.Throws<ArgumentException>(() => r.Update(Guid.Empty, 1, CanalVenta.Online, null));
            Assert.Throws<ArgumentException>(() => r.Update(VipId, 1, CanalVenta.Colaborador, null));
        }
    }
}
