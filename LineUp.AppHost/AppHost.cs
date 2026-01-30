using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("env");

var db = builder.AddPostgres("db");

var api = builder.AddProject<LineUp_Backend>("api")
    .WithReference(db)
    .WaitFor(db);

var reactApp = builder.AddViteApp("lineup-client", "../lineup-client")
    .WithPnpm()
    .WithExternalHttpEndpoints()
    .WithReference(api)
    .WithEnvironment("CI", "true")
    .WaitFor(api);    

builder.Build().Run();
