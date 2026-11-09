using FunEvents.Domain.Entities;
using Xunit;

namespace FunEvents.Tests.Domain
{
    public sealed class CategoriaTests
    {
        [Fact]
        public void CreaYTrimeaNombreYDescripcion()
        {
            var cat = new Categoria(Guid.NewGuid(), "  Música  ", "  Conciertos  ");

            Assert.Equal("Música", cat.Nombre);
            Assert.Equal("Conciertos", cat.Descripcion);
        }

        [Fact]
        public void AceptaDescripcionNula()
        {
            var cat = new Categoria(Guid.NewGuid(), "Música");
            Assert.Null(cat.Descripcion);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void RechazaNombreVacioEnCreateYUpdate(string nombre)
        {
            Assert.Throws<ArgumentException>(() => new Categoria(Guid.NewGuid(), nombre));
            Assert.Throws<ArgumentException>(() => new Categoria(Guid.NewGuid(), "Ok").Update(nombre, null));
        }
    }
}
