using NConnect.Shared.Infrastructure;
using NConnect.Shared.Infrastructure.Contexts;
using NConnect.Shared.Infrastructure.Modules.Loader;

var builder = WebApplication.CreateBuilder(args);

builder.AddInfrastructure();
    
builder.Services.AddModules(builder.Configuration);

var app = builder.Build();

app.UseInfrastructure();

app.UseModules();

app.Run();