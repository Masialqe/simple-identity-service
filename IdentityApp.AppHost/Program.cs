var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddPostgres("db")
        .WithPgAdmin()
        .WithDataVolume(isReadOnly: false);

var identityDb = database.AddDatabase("identityDb");

builder.AddProject<Projects.IdentityApp>("identityapp")
    .WithReference(identityDb);

builder.Build().Run();
