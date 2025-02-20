using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NConnect.Shared.Infrastructure.Modules.Loader;

public static class Extensions
{
    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureModuleLoader();
        ModuleLoader.AddModules(services, configuration);
        
        return services;
    }
    
    private static IServiceCollection ConfigureModuleLoader(this IServiceCollection services)
    {
        var loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
        
        ModuleLoader.ConfigureLogger(loggerFactory);

        return services;
    }
}