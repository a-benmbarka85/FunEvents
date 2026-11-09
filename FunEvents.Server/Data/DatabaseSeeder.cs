using FunEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Server.Data
{
    public static class DatabaseSeeder
    {
        private static readonly Guid AdminId = Guid.Parse("eb0f2a96-ef8c-4a37-9759-b1be38eb3504");
        private static readonly Guid Cliente1Id = Guid.Parse("72d5b565-df45-4235-8656-7871ad26e95c");
        private static readonly Guid Cliente2Id = Guid.Parse("33333333-3333-3333-3333-333333333333");

        private static readonly Guid PartnerOficialId = Guid.Parse("f1e2d3c4-b5a6-9786-5432-10fedcba9876");
        private static readonly Guid PartnerSecundarioId = Guid.Parse("f2e3d4c5-b6a7-9876-5432-10fedcba9877");
        private static readonly Guid PartnerOtrosId = Guid.Parse("f3e4d5c6-b7a8-9876-5432-10fedcba9878");

        private static readonly Guid EventoRockId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        private static readonly Guid EventoFutbolId = Guid.Parse("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        private static readonly Guid EventoTeatroId = Guid.Parse("c3d4e5f6-a7b8-9012-cdef-123456789012");
        private static readonly Guid EventoFestivalId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567891");
        private static readonly Guid EventoInviernoId = Guid.Parse("b1c2d3e4-f5a6-7890-abcd-ef1234567892");

        private static readonly Guid RockGeneralId = Guid.Parse("34197f02-aeba-476b-a5af-d8a54a96ee37");
        private static readonly Guid RockVipId = Guid.Parse("56fa8672-ec10-449e-b7d8-f7b6058b85cc");
        private static readonly Guid FutbolGeneralId = Guid.Parse("7c2c9d81-8b3d-497b-94df-d437efbca001");
        private static readonly Guid FutbolPlateaId = Guid.Parse("a45b7f12-9c16-4d2b-8fe5-1a89c2409fde");
        private static readonly Guid TeatroGeneralId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        private static readonly Guid TeatroPalcoId = Guid.Parse("de279402-fa81-4277-bc6d-4bfa56a64f33");
        private static readonly Guid FestivalGeneralId = Guid.Parse("41bb2f47-c0f5-4d7a-ba92-f04b122e23d1");
        private static readonly Guid FestivalVipId = Guid.Parse("e913bc54-2fa8-48b5-b778-9a3cbf53dd8c");
        private static readonly Guid InviernoGeneralId = Guid.Parse("bf81a3d5-e214-4115-bdbe-137bc6b5c00e");
        private static readonly Guid InviernoPlateaId = Guid.Parse("c61b52a3-f09c-4db3-bb6c-31a89b6fd16e");

        public static async Task SeedAsync(FunEventsDbContext context, CancellationToken cancellationToken = default)
        {
            if (!await context.Usuarios.AnyAsync(cancellationToken))
            {
                context.Usuarios.Add(new Usuario(AdminId, "Administrador Sistema", "admin@funevents.com", "Admin"));
                context.Usuarios.Add(new Usuario(Cliente1Id, "Cliente Online", "cliente1@funevents.com", "Cliente"));
                context.Usuarios.Add(new Usuario(Cliente2Id, "Cliente Oficina", "cliente2@funevents.com", "Cliente"));
            }

            if (!await context.Colaboradores.AnyAsync(cancellationToken))
            {
                context.Colaboradores.Add(new Colaborador(PartnerOficialId, "Partner Oficial", "oficial@partner.com"));
                context.Colaboradores.Add(new Colaborador(PartnerSecundarioId, "Partner segundario", "segundario@partner.com"));
                context.Colaboradores.Add(new Colaborador(PartnerOtrosId, "Otros", "otros@partner.com"));
            }

            if (!await context.Eventos.AnyAsync(cancellationToken))
            {
                context.Eventos.Add(new Evento(EventoRockId, "Evento Rock", DateTime.UtcNow.AddDays(30), 100));
                context.Eventos.Add(new Evento(EventoFutbolId, "Partido de futbol", DateTime.UtcNow.AddDays(45), 200));
                context.Eventos.Add(new Evento(EventoTeatroId, "Teatro", DateTime.UtcNow.AddDays(60), 40));
                context.Eventos.Add(new Evento(EventoFestivalId, "Festival de Verano", DateTime.UtcNow.AddDays(75), 150));
                context.Eventos.Add(new Evento(EventoInviernoId, "Concierto de Invierno", DateTime.UtcNow.AddDays(90), 120));
            }

            if (!await context.TiposEntrada.AnyAsync(cancellationToken))
            {
                context.TiposEntrada.Add(new TipoEntrada(RockGeneralId, EventoRockId, "General", 50m, "Entrada general para el evento Rock"));
                context.TiposEntrada.Add(new TipoEntrada(RockVipId, EventoRockId, "VIP", 120m, "Entrada VIP con acceso preferencial"));
                context.TiposEntrada.Add(new TipoEntrada(FutbolGeneralId, EventoFutbolId, "General", 80m, "Entrada general para el partido de futbol"));
                context.TiposEntrada.Add(new TipoEntrada(FutbolPlateaId, EventoFutbolId, "Platea", 150m, "Entrada de platea con vista al campo"));
                context.TiposEntrada.Add(new TipoEntrada(TeatroGeneralId, EventoTeatroId, "General", 40m, "Entrada general para teatro"));
                context.TiposEntrada.Add(new TipoEntrada(TeatroPalcoId, EventoTeatroId, "Palco", 90m, "Entrada de palco privado"));
                context.TiposEntrada.Add(new TipoEntrada(FestivalGeneralId, EventoFestivalId, "General", 60m, "Entrada general para el festival"));
                context.TiposEntrada.Add(new TipoEntrada(FestivalVipId, EventoFestivalId, "VIP", 130m, "Entrada VIP con acceso preferencial al festival"));
                context.TiposEntrada.Add(new TipoEntrada(InviernoGeneralId, EventoInviernoId, "General", 70m, "Entrada general para el concierto"));
                context.TiposEntrada.Add(new TipoEntrada(InviernoPlateaId, EventoInviernoId, "Platea", 160m, "Entrada de platea con vista al escenario"));
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
