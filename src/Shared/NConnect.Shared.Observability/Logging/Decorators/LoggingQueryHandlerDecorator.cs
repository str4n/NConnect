using Microsoft.Extensions.Logging;
using NConnect.Shared.Common;
using NConnect.Shared.Common.Abstractions.Queries;
using NConnect.Shared.Common.Attributes;
using NConnect.Shared.Contexts;

namespace NConnect.Shared.Observability.Logging.Decorators;

[Decorator]
internal sealed class LoggingQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> handler,
    ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger,
    IContextProvider contextProvider)
    : IQueryHandler<TQuery, TResult> where TQuery : class, IQuery<TResult>
{
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        var context = contextProvider.Current();
        var queryName = typeof(TQuery).Name;
        
        logger.LogInformation("Handling a command: {QueryName} [Activity ID: {ActivityId}, Message ID: {MessageId}, User ID: {UserId}']...",
            queryName, context.ActivityId, context.MessageId, context.UserId);
        
        var result = await handler.HandleAsync(query, cancellationToken);
        
        logger.LogInformation("Handled a query: {QueryName} [Activity ID: {ActivityId}, Message ID: {MessageId}, User ID: {UserId}]",
            queryName, context.ActivityId, context.MessageId, context.UserId);

        return result;
    }
}