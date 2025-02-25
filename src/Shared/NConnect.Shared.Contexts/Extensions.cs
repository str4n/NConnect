using Microsoft.Extensions.DependencyInjection;
using NConnect.Shared.Contexts.Accessors;
using NConnect.Shared.Contexts.Providers;

namespace NConnect.Shared.Contexts;

public static class Extensions
{
    public static IServiceCollection AddContexts(this IServiceCollection services)
    {
        services.AddSingleton<IContextProvider, ContextProvider>();
        services.AddSingleton<IContextAccessor, ContextAccessor>();

        return services;
    }
}