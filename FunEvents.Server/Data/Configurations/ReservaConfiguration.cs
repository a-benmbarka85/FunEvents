using FunEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunEvents.Server.Data.Configurations
{
    public sealed class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.TicketQuantity).IsRequired();
            builder.Property(r => r.ReservedAt).IsRequired();
            builder.Property(r => r.Canal).IsRequired();

            builder.HasOne<Evento>().WithMany().HasForeignKey(r => r.EventoId);
            builder.HasOne<Usuario>().WithMany().HasForeignKey(r => r.UsuarioId);
            builder.HasOne<Colaborador>().WithMany().HasForeignKey(r => r.ColaboradorId);
            builder.HasOne<TipoEntrada>().WithMany().HasForeignKey(r => r.TipoEntradaId);
        }
    }
}
