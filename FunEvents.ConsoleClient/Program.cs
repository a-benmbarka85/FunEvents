using System.Net.Http.Json;
using FunEvents.Application.Reservations.Commands;
using FunEvents.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

if (args.Length < 5
    || !Guid.TryParse(args[0], out var evId) || !Guid.TryParse(args[1], out var usId)
    || !Guid.TryParse(args[2], out var tipId) || !Guid.TryParse(args[3], out var colId)
    || !int.TryParse(args[4], out var qty) || qty <= 0)
{
    Console.WriteLine("Uso: dotnet run --project FunEvents.ConsoleClient <eventoId> <usuarioId> <tipoEntradaId> <colaboradorId> <cantidadTickets>");
    return;
}

var builder = Host.CreateApplicationBuilder(args);
var url = Environment.GetEnvironmentVariable("services__api__http__0") ?? "http://localhost:5361";
builder.Services.AddHttpClient("Api", c => c.BaseAddress = new Uri(url));

using var host = builder.Build();
await host.StartAsync();

var client = host.Services.GetRequiredService<IHttpClientFactory>().CreateClient("Api");
var cmd = new CreateReservationCommand(evId, usId, tipId, qty, CanalVenta.Colaborador, colId);

try
{
    var res = await client.PostAsJsonAsync("/api/reservations", cmd);
    if (res.IsSuccessStatusCode)
    {
        var data = await res.Content.ReadFromJsonAsync<ReservationResponse>();
        Console.WriteLine($"Reserva creada: {data?.ReservaId} ({data?.QuantityReserved} entradas)");
    }
    else
    {
        Console.WriteLine($"Fallo ({(int)res.StatusCode}): {await res.Content.ReadAsStringAsync()}");
    }
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Error de conexion en {url}: {ex.Message}");
}
finally
{
    await host.StopAsync();
}
