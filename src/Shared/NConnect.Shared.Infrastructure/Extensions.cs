using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NConnect.Shared.Infrastructure.Contexts;
using NConnect.Shared.Infrastructure.CQRS;
using NConnect.Shared.Infrastructure.Logging;

namespace NConnect.Shared.Infrastructure;

public static class Extensions
{
    private const string CorrelationIdKey = "correlation-id";

    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Host.UseLogging(builder.Configuration);
        
        builder.Services
            .AddLogging(builder.Configuration)
            .AddHttpContextAccessor()
            .AddContext()
            .AddAuthorization()
            .AddCommands()
            .AddQueries()
            .AddDispatcher();
        
        return builder;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });

        app.UseCorrelationId();
        app.UseContext();
        app.UseRouting();
        app.UseAuthorization();

        return app;
    }
    
    public static TOptions BindOptions<TOptions>(this IConfiguration configuration, string sectionName) where TOptions : class, new()
        => BindOptions<TOptions>(configuration.GetSection(sectionName));
    
    public static TOptions BindOptions<TOptions>(this IConfigurationSection section) where TOptions : class, new()
    {
        var options = new TOptions();
        section.Bind(options);

        return options;
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