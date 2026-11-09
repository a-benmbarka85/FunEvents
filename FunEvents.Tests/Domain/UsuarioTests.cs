using FunEvents.Domain.Entities;
using Xunit;

namespace FunEvents.Tests.Domain
{
    public sealed class UsuarioTests
    {
        [Fact]
        public void NormalizaNombreYEmailEnCreacion()
        {
            var ana = new Usuario(Guid.NewGuid(), "  Ana Perez  ", "  Ana@Empresa.COM ");

            Assert.Equal("Ana Perez", ana.Nombre);
            Assert.Equal("ana@empresa.com", ana.Email);
            Assert.Equal("Cliente", ana.Rol);
        }

        [Fact]
        public void UpdateNormalizaEmailYRol()
        {
            var ana = new Usuario(Guid.NewGuid(), "Ana", "ana@empresa.com");
            ana.Update("Ana", " ANA@EMPRESA.COM ", "Admin");

            Assert.Equal("ana@empresa.com", ana.Email);
            Assert.Equal("Admin", ana.Rol);
        }

        [Theory]
        [InlineData("  ", "ana@empresa.com")]
        [InlineData("Ana", "  ")]
        public void RechazaCamposVaciosEnCreacion(string nombre, string email)
        {
            Assert.Throws<ArgumentException>(() => new Usuario(Guid.NewGuid(), nombre, email));
        }
     
    }
}
