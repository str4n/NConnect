using Microsoft.Extensions.DependencyInjection;
using NConnect.Shared.Abstractions.Attributes;
using NConnect.Shared.Abstractions.CQRS;
using NConnect.Shared.Abstractions.CQRS.Commands;
using NConnect.Shared.Abstractions.CQRS.Queries;
using NConnect.Shared.Infrastructure.CQRS.Dispatchers;

namespace NConnect.Shared.Infrastructure.CQRS;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>))
                    .WithoutAttribute<DecoratorAttribute>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.AddSingleton<ICommandDispatcher, InMemoryCommandDispatcher>();

        return services;
    }

    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>))
                    .WithoutAttribute<DecoratorAttribute>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.AddSingleton<IQueryDispatcher, InMemoryQueryDispatcher>();

        return services;
    }

    public static IServiceCollection AddDispatcher(this IServiceCollection services)
        => services.AddSingleton<IDispatcher, InMemoryDispatcher>();
}