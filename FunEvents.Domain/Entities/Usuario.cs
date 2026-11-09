namespace FunEvents.Domain.Entities
{
    public sealed class Usuario
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string Rol { get; private set; } = "Cliente";
        public DateTime FechaRegistro { get; private set; }

        public Usuario(Guid id, string nombre, string email, string rol = "Cliente")
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email es requerido.", nameof(email));

            Id = id;
            Nombre = nombre.Trim();
            Email = email.Trim().ToLowerInvariant();
            Rol = rol.Trim();
            FechaRegistro = DateTime.UtcNow;
        }

        public void Update(string nombre, string email, string rol)
        {
            Nombre = nombre.Trim();
            Email = email.Trim().ToLowerInvariant();
            Rol = rol.Trim();
        }

        private Usuario() { }
    }
}