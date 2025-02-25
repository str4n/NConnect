using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NConnect.Shared.Api.CORS;
using NConnect.Shared.Api.Exceptions;
using NConnect.Shared.Api.OpenApi;
using NConnect.Shared.Common;
using NConnect.Shared.Contexts;
using NConnect.Shared.Observability.Logging;

namespace NConnect.Shared.Base;

public static class Extensions
{
    public static WebApplicationBuilder AddBaseFeatures(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        builder.Host.UseLogging(builder.Configuration);
        
        builder.Services
            .AddContexts()
            .AddHttpContextAccessor()
            .AddLogging(builder.Configuration)
            .AddCommonServices(builder.Configuration)
            .AddExceptionHandling()
            .AddCorsPolicy(builder.Configuration)
            .AddApiDocumentation();
        
        return builder;
    }

    public static WebApplication UseBaseFeatures(this WebApplication app)
    {
        app
            .UseForwardedHeaders()
            .UseCorsPolicy()
            .UseExceptionHandling()
            .UseHttpsRedirection()
            .UseRouting();

        app.MapDefaultEndpoints();

        app.MapGet("/", (AppInfo info) => Results.Ok(info));
        
        app.MapApiDocumentation();

        return app;
    }
}