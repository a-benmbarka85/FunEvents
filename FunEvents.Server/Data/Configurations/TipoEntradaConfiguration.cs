using FunEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunEvents.Server.Data.Configurations
{
    public sealed class TipoEntradaConfiguration : IEntityTypeConfiguration<TipoEntrada>
    {
        public void Configure(EntityTypeBuilder<TipoEntrada> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Precio).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(t => t.Descripcion).HasMaxLength(500);
            builder.Property(t => t.EventoId).IsRequired();

            builder.HasOne<Evento>().WithMany().HasForeignKey(t => t.EventoId);

            builder.HasIndex(t => new { t.EventoId, t.Nombre }).IsUnique();
        }
    }
}
