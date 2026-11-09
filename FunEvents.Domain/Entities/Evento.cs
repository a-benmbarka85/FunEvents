namespace FunEvents.Domain.Entities
{
    public sealed class Evento
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = null!;
        public DateTime Date { get; private set; }
        public int AvailableTickets { get; private set; }
        public uint Version { get; private set; }

        public Evento(Guid id, string title, DateTime date, int initialTickets)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("El título es requerido.", nameof(title));

            if (initialTickets < 0)
                throw new ArgumentException("Las entradas no pueden ser negativas.", nameof(initialTickets));

            Id = id;
            Title = title.Trim();
            Date = date;
            AvailableTickets = initialTickets;
        }

        public void ReserveTickets(int quantity)
        {
            if (Date < DateTime.UtcNow)
                throw new InvalidOperationException("No se pueden reservar entradas para eventos pasados.");

            if (quantity <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(quantity));

            if (AvailableTickets < quantity)
                throw new InvalidOperationException("No hay suficientes entradas disponibles.");

            AvailableTickets -= quantity;
            Version++;
        }

        public void ReleaseTickets(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(quantity));

            AvailableTickets += quantity;
        }

        public void Update(string title, DateTime date, int availableTickets)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("El título es requerido.", nameof(title));

            Title = title.Trim();
            Date = date;
            AvailableTickets = availableTickets;
            Version++;
        }

        private Evento() { }
    }
}