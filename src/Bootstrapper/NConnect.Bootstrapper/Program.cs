using NConnect.Shared.Infrastructure;
using NConnect.Shared.Infrastructure.Contexts;
using NConnect.Shared.Infrastructure.Modules.Loader;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddModules(builder.Configuration);

var app = builder.Build();

app.UseInfrastructure();

app.UseModules();

app.Run();