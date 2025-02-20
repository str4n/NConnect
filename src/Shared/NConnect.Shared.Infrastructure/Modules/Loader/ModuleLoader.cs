using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NConnect.Shared.Abstractions.Modules;

namespace NConnect.Shared.Infrastructure.Modules.Loader;

public static class ModuleLoader
{
    private static readonly List<IModule> Modules = new();
    private static ILogger? _logger;
    
    public static void Load<TModule>() where TModule : IModule
    {
        var module = Activator.CreateInstance<TModule>();
        Modules.Add(module);
    }

    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        Modules.ForEach(module =>
        {
            module.Add(services, configuration);
            _logger?.LogInformation($"Added services for module {module.Name}");
        });
        
        return services;
    }

    public static WebApplication UseModules(this WebApplication app)
    {
        Modules.ForEach(module =>
        {
            module.Use(app);
            _logger?.LogInformation($"Added middlewares for module {module.Name}");
        });

        return app;
    }

    internal static void ConfigureLogger(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(nameof(ModuleLoader));
    }
}