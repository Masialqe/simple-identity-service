var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.IdentityApp>("identityapp");

builder.Build().Run();
