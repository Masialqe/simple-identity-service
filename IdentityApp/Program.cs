using IdentityApp.Common.Configuration;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Users.Validators;
using IdentityApp.Extensions;
using IdentityApp.Middleware;
using IdentityApp.Endpoints;
using IdentityApp.Managers;
using FluentValidation;
using Serilog;
using IdentityApp.Shared.Infrastructure;
using IdentityApp.Shared.Infrastructure.Data;

//Aspire Postgre + database configure
//Add password policy

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.AddEndpoints();
builder.Services.AddConfiguredOptions();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddDbContext<IdentityDbContext>(b =>
{
    b.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),options =>
    {
        options.EnableRetryOnFailure();
        options.CommandTimeout(15);
    });
});

builder.ConfigureSerilog();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddManagers();
builder.Services.AddValidators();
builder.Services.AddRepositories();

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