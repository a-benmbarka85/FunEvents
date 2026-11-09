namespace FunEvents.Domain.Entities
{
    public sealed class Colaborador
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Email { get; private set; } = null!;

        public Colaborador(Guid id, string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre es requerido.", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email es requerido.", nameof(email));

            Id = id;
            Name = name.Trim();
            Email = email.Trim().ToLowerInvariant();
        }

        public void Update(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre es requerido.", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email es requerido.", nameof(email));

            Name = name.Trim();
            Email = email.Trim().ToLowerInvariant();
        }

        private Colaborador() { }
    }
}