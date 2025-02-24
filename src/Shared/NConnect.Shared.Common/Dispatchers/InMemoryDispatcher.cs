using NConnect.Shared.Common.Abstractions.Commands;
using NConnect.Shared.Common.Abstractions.Dispatchers;
using NConnect.Shared.Common.Abstractions.Queries;

namespace NConnect.Shared.Common.Dispatchers;

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