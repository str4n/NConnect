using NConnect.Shared.Common.Abstractions.Commands;
using NConnect.Shared.Common.Abstractions.Queries;

namespace NConnect.Shared.Common.Abstractions.Dispatchers;

public interface IDispatcher
{
    Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand;
    Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}