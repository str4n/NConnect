using NConnect.Shared.Infrastructure.Modules;
using NConnect.Shared.Infrastructure.Modules.Loader;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureModuleLoader()
    .AddModules(builder.Configuration);

var app = builder.Build();

app.UseModules();

app.Run();