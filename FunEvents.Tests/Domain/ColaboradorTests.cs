using FunEvents.Domain.Entities;
using Xunit;

namespace FunEvents.Tests.Domain
{
    public sealed class ColaboradorTests
    {
        [Fact]
        public void NormalizaNombreYEmail()
        {
            var colab = new Colaborador(Guid.NewGuid(), "  Joao Silva  ", "  Joao@Empresa.COM ");

            Assert.Equal("Joao Silva", colab.Name);
            Assert.Equal("joao@empresa.com", colab.Email);
        }

        [Fact]
        public void UpdateNormalizaEmail()
        {
            var colab = new Colaborador(Guid.NewGuid(), "Joao", "joao@empresa.com");
            colab.Update("Joao S", "  NOVO@EMPRESA.COM  ");
            Assert.Equal("novo@empresa.com", colab.Email);
        }

        [Theory]
        [InlineData("", "a@b.com")]
        [InlineData("Joao", "")]
        [InlineData("  ", "a@b.com")]
        [InlineData("Joao", "  ")]
        public void RechazaCamposVacios(string nombre, string email)
        {
            Assert.Throws<ArgumentException>(() => new Colaborador(Guid.NewGuid(), nombre, email));
            Assert.Throws<ArgumentException>(() => new Colaborador(Guid.NewGuid(), "Ok", "ok@x.com").Update(nombre, email));
        }
    }
}
