using NConnect.Shared.Abstractions.CQRS;
using NConnect.Shared.Abstractions.CQRS.Commands;
using NConnect.Shared.Abstractions.CQRS.Queries;

namespace NConnect.Shared.Infrastructure.CQRS.Dispatchers;

internal sealed class InMemoryDispatcher(
    IQueryDispatcher queryDispatcher, 
    ICommandDispatcher commandDispatcher)
    : IDispatcher
{
    public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand
        => await commandDispatcher.DispatchAsync(command, cancellationToken);

    public async Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        => await queryDispatcher.DispatchAsync(query, cancellationToken);
}