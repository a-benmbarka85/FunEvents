using FunEvents.Server.Data;

namespace FunEvents.Server.Endpoints
{
    public static class SeedEndpoints
    {
        public static void MapSeedEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/seed", async (FunEventsDbContext context, CancellationToken ct) =>
            {
                await DatabaseSeeder.SeedAsync(context, ct);
                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithTags("Seed");
        }
    }
}