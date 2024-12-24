using IdentityApp.Shared.Infrastructure.Data;
using IdentityApp.Shared.Infrastructure;
using IdentityApp.Shared.Managers;
using IdentityApp.Configuration;
using IdentityApp.Extensions;
using IdentityApp.Middleware;
using IdentityApp.Endpoints;
using IdentityApp.Users;
using FluentValidation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.AddEndpoints();
builder.Services.AddConfiguredOptions();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.AddNpgsqlDbContext<IdentityDbContext>(connectionName: "identityDb");

builder.ConfigureSerilog();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddManagers();
builder.Services.AddUserFeatures();
builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.MapEndpoints();

app.UseExceptionHandler();

app.UseMiddleware<RequestLoggingExtensionMiddleware>();
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();  

app.UseMiddleware<SecurityHeadersMiddleware>();

app.RunAplication();