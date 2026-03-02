using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var postgresdb = postgres.AddDatabase("postgresdb");

var api = builder.AddProject<LineUp_Backend>("api")
    .WaitFor(postgresdb)
    .WithReference(postgresdb);

var web = builder.AddViteApp("web", "../lineup-client")
    .WithExternalHttpEndpoints()
    .WithPnpm()
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
