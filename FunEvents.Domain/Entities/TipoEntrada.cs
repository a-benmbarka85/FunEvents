namespace FunEvents.Domain.Entities
{
    public sealed class TipoEntrada
    {
        public Guid Id { get; private set; }
        public Guid EventoId { get; private set; }
        public string Nombre { get; private set; } = null!;
        public decimal Precio { get; private set; }
        public string? Descripcion { get; private set; }

        public TipoEntrada(Guid id, Guid eventoId, string nombre, decimal precio, string? descripcion = null)
        {
            if (eventoId == Guid.Empty)
                throw new ArgumentException("El evento es requerido.", nameof(eventoId));

            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));

            if (precio < 0)
                throw new ArgumentException("El precio no puede ser negativo.", nameof(precio));

            Id = id;
            EventoId = eventoId;
            Nombre = nombre.Trim();
            Precio = precio;
            Descripcion = descripcion?.Trim();
        }

        public void Update(string nombre, decimal precio, string? descripcion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));

            if (precio < 0)
                throw new ArgumentException("El precio no puede ser negativo.", nameof(precio));

            Nombre = nombre.Trim();
            Precio = precio;
            Descripcion = descripcion?.Trim();
        }

        public bool BelongsTo(Guid eventoId) => EventoId == eventoId;

        private TipoEntrada() { }
    }
}