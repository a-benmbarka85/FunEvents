namespace FunEvents.Domain.Entities
{
    public sealed class Categoria
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; } = null!;
        public string? Descripcion { get; private set; }

        public Categoria(Guid id, string nombre, string? descripcion = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));

            Id = id;
            Nombre = nombre.Trim();
            Descripcion = descripcion?.Trim();
        }

        public void Update(string nombre, string? descripcion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));

            Nombre = nombre.Trim();
            Descripcion = descripcion?.Trim();
        }

        private Categoria() { }
    }
}