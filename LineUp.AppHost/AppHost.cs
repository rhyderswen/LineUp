using LineUp.AppHost;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("env");

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

var postgresdb = postgres.AddDatabase("lineupdb");

var api = builder.AddProject<LineUp_Backend>("api")
    .WaitFor(postgresdb)
    .WithReference(postgresdb);

if (!builder.ExecutionContext.IsRunMode)
{
    api.WithEndpoint("http", e =>
    {
        e.Port = 3010;
        e.IsExternal = true;
    });
}

IResourceBuilder<IResourceWithEndpoints> web;

if (builder.ExecutionContext.IsRunMode)
{
    web = builder.AddViteApp("web", "../lineup-client")
        .WithPnpm()
        .WithHttpEndpoint(port: 5173, env: "PORT")
        .WithExternalHttpEndpoints()
        .WithReference(api)
        .WaitFor(api)
        .AsDeployableService();
}
else
{
    web = builder.AddDockerfile("web", "../lineup-client")
        .WithHttpEndpoint(port: 8080, targetPort: 80, env: "PORT")
        .WithExternalHttpEndpoints()
        .WithEnvironment("VITE_API_URL", api.GetEndpoint("http"))
        .WithReference(api)
        .WaitFor(api);
}

builder.Build().Run();
