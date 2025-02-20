using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NConnect.Shared.Infrastructure.Modules.Loader;

public static class Extensions
{
    public static IServiceCollection ConfigureModuleLoader(this IServiceCollection services)
    {
        var loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
        
        ModuleLoader.ConfigureLogger(loggerFactory);

        return services;
    }
}