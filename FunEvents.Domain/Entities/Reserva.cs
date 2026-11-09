namespace FunEvents.Domain.Entities
{
    public sealed class Reserva
    {
        public Guid Id { get; private set; }
        public Guid EventoId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public Guid? ColaboradorId { get; private set; }
        public Guid TipoEntradaId { get; private set; }
        public CanalVenta Canal { get; private set; }
        public int TicketQuantity { get; private set; }
        public DateTime ReservedAt { get; private set; }

        public Reserva(
            Guid id,
            Guid eventoId,
            Guid usuarioId,
            Guid tipoEntradaId,
            int ticketQuantity,
            CanalVenta canal = CanalVenta.Online,
            Guid? colaboradorId = null)
        {
            if (eventoId == Guid.Empty)
                throw new ArgumentException("El evento es requerido.", nameof(eventoId));

            if (usuarioId == Guid.Empty)
                throw new ArgumentException("El usuario es requerido.", nameof(usuarioId));

            if (tipoEntradaId == Guid.Empty)
                throw new ArgumentException("El tipo de entrada es requerido.", nameof(tipoEntradaId));

            if (ticketQuantity <= 0)
                throw new ArgumentException("La cantidad de entradas debe ser mayor a cero.", nameof(ticketQuantity));

            if (canal == CanalVenta.Colaborador && colaboradorId is null)
                throw new ArgumentException("El colaborador es obligatorio para el canal Colaborador.", nameof(colaboradorId));

            Id = id;
            EventoId = eventoId;
            UsuarioId = usuarioId;
            TipoEntradaId = tipoEntradaId;
            TicketQuantity = ticketQuantity;
            Canal = canal;
            ColaboradorId = colaboradorId;
            ReservedAt = DateTime.UtcNow;
        }

        public void Update(Guid tipoEntradaId, int ticketQuantity, CanalVenta canal, Guid? colaboradorId)
        {
            if (tipoEntradaId == Guid.Empty)
                throw new ArgumentException("El tipo de entrada es requerido.", nameof(tipoEntradaId));

            if (ticketQuantity <= 0)
                throw new ArgumentException("La cantidad de entradas debe ser mayor a cero.", nameof(ticketQuantity));

            if (canal == CanalVenta.Colaborador && colaboradorId is null)
                throw new ArgumentException("El colaborador es obligatorio para el canal Colaborador.", nameof(colaboradorId));

            TipoEntradaId = tipoEntradaId;
            TicketQuantity = ticketQuantity;
            Canal = canal;
            ColaboradorId = colaboradorId;
        }

        private Reserva() { }
    }
}