using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace FunEvents.Server.Security
{
    public sealed class ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IConfiguration configuration)
        : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        public const string SchemeName = "ApiKey";
        public const string HeaderName = "X-Api-Key";

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var expected = configuration["Security:AdminApiKey"];
            if (string.IsNullOrWhiteSpace(expected))
                return Task.FromResult(AuthenticateResult.Fail("La clave de administración no está configurada."));

            if (!Request.Headers.TryGetValue(HeaderName, out var provided) || provided.Count != 1)
                return Task.FromResult(AuthenticateResult.NoResult());

            if (!FixedTimeEquals(provided[0]!, expected))
                return Task.FromResult(AuthenticateResult.Fail("La clave de administración no es válida."));

            var identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Role, "Admin")
            ], SchemeName);

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        private static bool FixedTimeEquals(string provided, string expected) =>
            CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(provided),
                Encoding.UTF8.GetBytes(expected));
    }
}