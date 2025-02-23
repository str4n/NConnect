using NConnect.Aspire.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddServices();

await builder.Build().RunAsync();