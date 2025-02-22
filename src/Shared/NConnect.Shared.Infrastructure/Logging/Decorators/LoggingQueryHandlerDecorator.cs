using Microsoft.Extensions.Logging;
using NConnect.Shared.Abstractions.Attributes;
using NConnect.Shared.Abstractions.Contexts;
using NConnect.Shared.Abstractions.CQRS.Queries;

namespace NConnect.Shared.Infrastructure.Logging.Decorators;

[Decorator]
internal sealed class LoggingQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> handler,
    ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger,
    IContext context)
    : IQueryHandler<TQuery, TResult> where TQuery : class, IQuery<TResult>
{
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        var module = query.GetModuleName();
        var name = query.GetType().Name;
        var requestId = context.RequestId;
        var correlationId = context.CorrelationId;
        var traceId = context.TraceId;
        var userId = context.Identity.Id;
        
        logger.LogInformation("Handling a query: {Name} ({Module}) [Request ID: {RequestId}, Correlation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}]'...",
            name, module, requestId, correlationId, traceId, userId);

        var result = await handler.HandleAsync(query, cancellationToken);
        
        logger.LogInformation("Handled a query: {Name} ({Module}) [Request ID: {RequestId}, Correlation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}']",
            name, module, requestId, correlationId, traceId, userId);
        
        return result;
    }
}