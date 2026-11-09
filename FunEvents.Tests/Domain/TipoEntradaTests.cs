using FunEvents.Domain.Entities;
using Xunit;

namespace FunEvents.Tests.Domain
{
    public sealed class TipoEntradaTests
    {
        private static readonly Guid FestId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890");

        [Fact]
        public void BelongsToComparaEventoId()
        {
            var vip = new TipoEntrada(Guid.NewGuid(), FestId, "VIP", 150m);
            Assert.True(vip.BelongsTo(FestId));
            Assert.False(vip.BelongsTo(Guid.NewGuid()));
        }

        [Fact]
        public void CreaConPrecioCeroYTrimea()
        {
            var t = new TipoEntrada(Guid.NewGuid(), FestId, "  Cortesia  ", 0m, "  Free  ");
            Assert.Equal("Cortesia", t.Nombre);
            Assert.Equal("Free", t.Descripcion);
            Assert.Equal(0m, t.Precio);
        }

        [Fact]
        public void RechazaDatosInvalidosEnCreacion()
        {
            Assert.Throws<ArgumentException>(() => new TipoEntrada(Guid.NewGuid(), Guid.Empty, "General", 50m));
            Assert.Throws<ArgumentException>(() => new TipoEntrada(Guid.NewGuid(), FestId, "  ", 50m));
            Assert.Throws<ArgumentException>(() => new TipoEntrada(Guid.NewGuid(), FestId, "General", -10m));
        }

        [Fact]
        public void RechazaDatosInvalidosEnUpdate()
        {
            var t = new TipoEntrada(Guid.NewGuid(), FestId, "General", 50m);
            Assert.Throws<ArgumentException>(() => t.Update("  ", 10m, null));
            Assert.Throws<ArgumentException>(() => t.Update("General", -5m, null));
        }
    }
}
