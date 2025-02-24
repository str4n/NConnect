using NConnect.Shared.Base;

var builder = WebApplication.CreateBuilder(args);

builder.AddBaseFeatures();

var app = builder.Build();

app.UseBaseFeatures();

app.Run();