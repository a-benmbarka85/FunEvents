using FunEvents.Application.Common.Exceptions;
using FunEvents.Application.Common.Interfaces;
using FunEvents.Application.Reservations.Commands;
using FunEvents.Domain.Entities;
using Moq;
using Xunit;

namespace FunEvents.Tests.Application
{
    public class CreateReservationUseCaseTests
    {
        private static readonly Guid VipId = Guid.Parse("34197f02-aeba-476b-a5af-d8a54a96ee37");

        [Fact]
        public async Task PersisteReservaYDescuentaStock()
        {
            var fest = new Evento(Guid.NewGuid(), "Lollapalooza", DateTime.UtcNow.AddDays(10), 100);
            var ana = new Usuario(Guid.NewGuid(), "Ana", "ana@test.com");
            var vip = new TipoEntrada(VipId, fest.Id, "VIP", 150m);
            var (sut, reservas, eventos, uow) = Build(fest, ana, vip);

            var res = await sut.ExecuteAsync(new CreateReservationCommand(
                fest.Id, ana.Id, VipId, 3, CanalVenta.Colaborador, Guid.NewGuid()));

            Assert.Equal(fest.Id, res.EventoId);
            Assert.Equal(3, res.QuantityReserved);
            Assert.Equal(97, fest.AvailableTickets);
            reservas.Verify(r => r.AddAsync(It.IsAny<Reserva>(), It.IsAny<CancellationToken>()), Times.Once);
            eventos.Verify(e => e.UpdateAsync(fest, It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task FallaSiEventoNoExiste()
        {
            var (sut, _, _, _) = Build(null, new Usuario(Guid.NewGuid(), "Ana", "ana@test.com"),
                new TipoEntrada(VipId, Guid.NewGuid(), "VIP", 150m));

            await Assert.ThrowsAsync<NotFoundException>(() =>
                sut.ExecuteAsync(new CreateReservationCommand(Guid.NewGuid(), Guid.NewGuid(), VipId, 1)));
        }

        [Fact]
        public async Task FallaSiUsuarioNoExiste()
        {
            var fest = new Evento(Guid.NewGuid(), "Lolla", DateTime.UtcNow.AddDays(10), 100);
            var (sut, _, _, _) = Build(fest, null, new TipoEntrada(VipId, fest.Id, "VIP", 150m));

            await Assert.ThrowsAsync<NotFoundException>(() =>
                sut.ExecuteAsync(new CreateReservationCommand(fest.Id, Guid.NewGuid(), VipId, 1)));
        }

        [Fact]
        public async Task FallaSiTipoNoExiste()
        {
            var fest = new Evento(Guid.NewGuid(), "Lolla", DateTime.UtcNow.AddDays(10), 100);
            var ana = new Usuario(Guid.NewGuid(), "Ana", "ana@test.com");
            var (sut, _, _, _) = Build(fest, ana, null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                sut.ExecuteAsync(new CreateReservationCommand(fest.Id, ana.Id, VipId, 1)));
        }

        [Fact]
        public async Task FallaSiTipoEsDeOtroEvento()
        {
            var fest = new Evento(Guid.NewGuid(), "Lolla", DateTime.UtcNow.AddDays(10), 100);
            var ana = new Usuario(Guid.NewGuid(), "Ana", "ana@test.com");
            var tipoAjeno = new TipoEntrada(VipId, Guid.NewGuid(), "VIP", 150m);
            var (sut, _, _, _) = Build(fest, ana, tipoAjeno);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                sut.ExecuteAsync(new CreateReservationCommand(fest.Id, ana.Id, VipId, 1)));
        }

        [Fact]
        public async Task FallaSiNoHayStock()
        {
            var fest = new Evento(Guid.NewGuid(), "Sold Out Fest", DateTime.UtcNow.AddDays(10), 1);
            var ana = new Usuario(Guid.NewGuid(), "Ana", "ana@test.com");
            var general = new TipoEntrada(VipId, fest.Id, "General", 50m);
            var (sut, _, _, _) = Build(fest, ana, general);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                sut.ExecuteAsync(new CreateReservationCommand(fest.Id, ana.Id, VipId, 2)));
        }

        private static (CreateReservationUseCase Sut, Mock<IReservationRepository> Reservas, Mock<IEventRepository> Eventos, Mock<IUnitOfWork> Uow)
            Build(Evento? evento, Usuario? usuario, TipoEntrada? tipo)
        {
            var evRepo = new Mock<IEventRepository>();
            evRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(evento);

            var usrRepo = new Mock<IUsuarioRepository>();
            usrRepo.Setup(u => u.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(usuario);

            var tipoRepo = new Mock<ITipoEntradaRepository>();
            tipoRepo.Setup(t => t.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(tipo);

            var resRepo = new Mock<IReservationRepository>();
            var uow = new Mock<IUnitOfWork>();

            return (new CreateReservationUseCase(evRepo.Object, tipoRepo.Object, usrRepo.Object, resRepo.Object, uow.Object), resRepo, evRepo, uow);
        }
    }
}
