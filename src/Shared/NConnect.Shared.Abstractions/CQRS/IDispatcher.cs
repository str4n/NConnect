using NConnect.Shared.Abstractions.CQRS.Commands;
using NConnect.Shared.Abstractions.CQRS.Queries;

namespace NConnect.Shared.Abstractions.CQRS;

public interface IDispatcher
{
    Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand;
    Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}