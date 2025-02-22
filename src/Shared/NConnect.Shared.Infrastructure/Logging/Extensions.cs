using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NConnect.Shared.Abstractions.CQRS.Commands;
using NConnect.Shared.Abstractions.CQRS.Queries;
using NConnect.Shared.Infrastructure.Logging.Decorators;
using Serilog;
using Serilog.Events;

namespace NConnect.Shared.Infrastructure.Logging;

internal static class Extensions
{
    private const string SerilogSectionName = "logger";
    private const string AppSectionName = "app";

    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SerilogOptions>(configuration.GetSection(SerilogSectionName));
        services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        services.TryDecorate(typeof(IQueryHandler<,>), typeof(LoggingQueryHandlerDecorator<,>));

        return services;
    }

    public static IHostBuilder UseLogging(this IHostBuilder host, IConfiguration configuration)
    {
        var serilogOptions = configuration.BindOptions<SerilogOptions>(SerilogSectionName);
        var appOptions = configuration.BindOptions<AppOptions>(AppSectionName);

        host.UseSerilog((ctx, cfg) =>
        {
            var level = GetLogEventLevel(serilogOptions.Level);
            
            cfg.Enrich.FromLogContext()
                .MinimumLevel.Is(level)
                .Enrich.WithProperty("environment", ctx.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("application", appOptions.Name)
                .Enrich.WithProperty("version", appOptions.Version);
            
            if (serilogOptions.Seq.Enabled)
            {
                cfg.WriteTo.Seq(serilogOptions.Seq.ConnectionString);
            }

            if (serilogOptions.Console.Enabled)
            {
                cfg.WriteTo.Console(outputTemplate: serilogOptions.Console.Template);
            }
        });

        return host;
    }
    
    private static LogEventLevel GetLogEventLevel(string level)
        => Enum.TryParse<LogEventLevel>(level, true, out var logLevel)
            ? logLevel
            : LogEventLevel.Information;
}