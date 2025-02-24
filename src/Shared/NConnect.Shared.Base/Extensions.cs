using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using NConnect.Shared.Api.CORS;
using NConnect.Shared.Api.Exceptions;
using NConnect.Shared.Api.OpenApi;
using NConnect.Shared.Common;

namespace NConnect.Shared.Base;

public static class Extensions
{
    public static IHostApplicationBuilder AddBaseFeatures(this IHostApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        builder.Services
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