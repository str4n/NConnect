using NConnect.Shared.Base;

var builder = WebApplication.CreateBuilder(args);

builder.AddBaseFeatures();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

var app = builder.Build();

app.UseBaseFeatures();

app.MapReverseProxy();

app.Run();