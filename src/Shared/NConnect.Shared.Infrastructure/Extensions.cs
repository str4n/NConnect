using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NConnect.Shared.Infrastructure.Contexts;

namespace NConnect.Shared.Infrastructure;

public static class Extensions
{
    private const string CorrelationIdKey = "correlation-id";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddContext();
        services.AddAuthorization();
        
        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });
        
        app.UseContext();
        app.UseCorrelationId();
        app.UseRouting();
        app.UseAuthorization();

        return app;
    }
    
    internal static Guid? TryGetCorrelationId(this HttpContext context)
        => context.Items.TryGetValue(CorrelationIdKey, out var id) ? (Guid) id! : null;
    
    private static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        => app.Use((ctx, next) =>
        {
            ctx.Items.Add(CorrelationIdKey, Guid.NewGuid());
            return next();
        });
}