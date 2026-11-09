using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres", port: 5433)
    .WithPgAdmin(pgAdmin => pgAdmin
        .WithEnvironment("PGADMIN_DEFAULT_EMAIL", builder.Configuration["PGADMIN_DEFAULT_EMAIL"] ?? Environment.GetEnvironmentVariable("PGADMIN_DEFAULT_EMAIL") ?? "admin@funevents.com")
        .WithEnvironment("PGADMIN_DEFAULT_PASSWORD", builder.Configuration["PGADMIN_DEFAULT_PASSWORD"] ?? Environment.GetEnvironmentVariable("PGADMIN_DEFAULT_PASSWORD") ?? "funevents-dev"))
    .AddDatabase("postgresdb");

var apiService = builder.AddProject<FunEvents_Server>("api")
    .WithReference(postgres)
    .WaitFor(postgres);

builder.AddProject<FunEvents_ConsoleClient>("consoleclient")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
