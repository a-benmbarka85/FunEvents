namespace FunEvents.Domain.Entities
{
    public sealed class Pago
    {
        public Guid Id { get; private set; }
        public Guid ReservaId { get; private set; }
        public decimal Monto { get; private set; }
        public string MetodoPago { get; private set; } = null!;
        public string Estado { get; private set; } = null!;
        public DateTime FechaPago { get; private set; }

        public Pago(Guid id, Guid reservaId, decimal monto, string metodoPago, string estado, DateTime fechaPago)
        {
            if (reservaId == Guid.Empty)
                throw new ArgumentException("La reserva es requerida.", nameof(reservaId));

            if (monto < 0)
                throw new ArgumentException("El monto no puede ser negativo.", nameof(monto));

            if (string.IsNullOrWhiteSpace(metodoPago))
                throw new ArgumentException("El método de pago es requerido.", nameof(metodoPago));

            if (string.IsNullOrWhiteSpace(estado))
                throw new ArgumentException("El estado es requerido.", nameof(estado));

            Id = id;
            ReservaId = reservaId;
            Monto = monto;
            MetodoPago = metodoPago.Trim();
            Estado = estado.Trim();
            FechaPago = fechaPago;
        }

        public void Update(decimal monto, string metodoPago, string estado, DateTime fechaPago)
        {
            Monto = monto;
            MetodoPago = metodoPago.Trim();
            Estado = estado.Trim();
            FechaPago = fechaPago;
        }

        private Pago() { }
    }
}