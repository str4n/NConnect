using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NConnect.Shared.Common;
using NConnect.Shared.Common.Abstractions.Commands;
using NConnect.Shared.Common.Abstractions.Queries;
using NConnect.Shared.Observability.Logging.Decorators;
using Serilog;
using Serilog.Events;

namespace NConnect.Shared.Observability.Logging;

public static class Extensions
{
    private const string SerilogSectionName = "Logger";
    private const string AppSectionName = "App";

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
                .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("Application", appOptions.Name)
                .Enrich.WithProperty("Version", appOptions.Version);

            if (serilogOptions.Console.Enabled)
            {
                cfg.WriteTo.Console(outputTemplate: serilogOptions.Console.Template);
            }

            if (serilogOptions.OpenTelemetry.Enabled)
            {
                cfg.WriteTo.OpenTelemetry();
            }
        });

        return host;
    }
    
    private static LogEventLevel GetLogEventLevel(string level)
        => Enum.TryParse<LogEventLevel>(level, true, out var logLevel)
            ? logLevel
            : LogEventLevel.Information;
}