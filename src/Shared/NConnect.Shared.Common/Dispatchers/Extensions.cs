using Microsoft.Extensions.DependencyInjection;
using NConnect.Shared.Common.Abstractions.Commands;
using NConnect.Shared.Common.Abstractions.Dispatchers;
using NConnect.Shared.Common.Abstractions.DomainEvents;
using NConnect.Shared.Common.Abstractions.Queries;
using NConnect.Shared.Common.Attributes;
using NConnect.Shared.Common.Dispatchers.Commands;
using NConnect.Shared.Common.Dispatchers.DomainEvents;
using NConnect.Shared.Common.Dispatchers.Queries;

namespace NConnect.Shared.Common.Dispatchers;

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

    public static IServiceCollection AddDomainEvents(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>))
                    .WithoutAttribute<DecoratorAttribute>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.AddSingleton<IDomainEventDispatcher, InMemoryDomainEventDispatcher>();

        return services;
    }

    public static IServiceCollection AddDispatcher(this IServiceCollection services)
        => services.AddSingleton<IDispatcher, InMemoryDispatcher>();
}