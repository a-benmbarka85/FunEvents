using System.Text.Json.Serialization;
using FunEvents.Application.Common.Interfaces;
using FunEvents.Application.Reservations.Commands;
using FunEvents.Server.Data;
using FunEvents.Server.Data.Repositories;
using FunEvents.Server.Endpoints;
using FunEvents.Server.Errors;
using FunEvents.Server.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceDiscovery();

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Name = ApiKeyAuthenticationHandler.HeaderName,
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services
    .AddAuthentication(ApiKeyAuthenticationHandler.SchemeName)
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationHandler.SchemeName, null);
builder.Services.AddAuthorization();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var connectionString = builder.Configuration.GetConnectionString("postgresdb")
    ?? throw new InvalidOperationException("Falta la cadena de conexión 'postgresdb'.");

builder.Services.AddDbContext<FunEventsDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPagoRepository, PagoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITipoEntradaRepository, TipoEntradaRepository>();
builder.Services.AddScoped<CreateReservationUseCase>();
builder.Services.AddScoped<UpdateReservationUseCase>();
builder.Services.AddScoped<DeleteReservationUseCase>();

var app = builder.Build();

app.UseExceptionHandler();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FunEventsDbContext>();
    await context.Database.MigrateAsync();

    if (app.Environment.IsDevelopment())
        await DatabaseSeeder.SeedAsync(context);
}

app.UseAuthentication();
app.UseAuthorization();

app.MapReservationEndpoints();
app.MapReservaEndpoints();
app.MapColaboradorEndpoints();
app.MapEventoEndpoints();
app.MapCategoriaEndpoints();
app.MapTipoEntradaEndpoints();
app.MapPagoEndpoints();
app.MapUsuarioEndpoints();
app.MapSeedEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();