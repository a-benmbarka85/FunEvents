using FunEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunEvents.Server.Data.Configurations;

public sealed class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Monto).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(p => p.MetodoPago).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Estado).IsRequired().HasMaxLength(50);
        builder.Property(p => p.FechaPago).IsRequired();

        builder.HasOne<Reserva>()
            .WithMany()
            .HasForeignKey(p => p.ReservaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
