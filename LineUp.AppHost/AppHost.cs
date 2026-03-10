using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume();
var postgresdb = postgres.AddDatabase("postgresdb");

var api = builder.AddProject<LineUp_Backend>("api")
    .WaitFor(postgresdb)
    .WithReference(postgresdb);

var web = builder.AddViteApp("web", "../lineup-client")
    .WithEnvironment("PORT", "5173")
    .WithExternalHttpEndpoints()
    .WithEndpoint("http", endpointAnnotation => endpointAnnotation.Port = 5173)
    .WithPnpm()
    .WithReference(api)
    .WaitFor(api);

builder.Eventing.Subscribe<ResourceEndpointsAllocatedEvent>((e, ct) => {
    switch (e.Resource.Name)
    {
        case "api":
            {
                var endpoint = api.GetEndpoint("http");
                Console.WriteLine($"Backend: {endpoint.Url}");
                Console.WriteLine($"Scalar: {endpoint.Url}/scalar");
                break;
            }
        case "web":
            {
                var endpoint = web.GetEndpoint("http");
                Console.WriteLine($"Frontend: {endpoint.Url}");
                break;
            }
    }
    return Task.CompletedTask;
});

builder.Build().Run();
