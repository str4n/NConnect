using Microsoft.Extensions.Logging;
using NConnect.Shared.Abstractions.Attributes;
using NConnect.Shared.Abstractions.Contexts;
using NConnect.Shared.Abstractions.CQRS.Commands;

namespace NConnect.Shared.Infrastructure.Logging.Decorators;

[Decorator]
internal sealed class LoggingCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> handler,
    ILogger<LoggingCommandHandlerDecorator<TCommand>> logger,
    IContext context)
    : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        var module = command.GetModuleName();
        var name = command.GetType().Name;
        var requestId = context.RequestId;
        var correlationId = context.CorrelationId;
        var traceId = context.TraceId;
        var userId = context.Identity.Id;
        
        logger.LogInformation("Handling a command: {Name} ({Module}) [Request ID: {RequestId}, Correlation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}]'...",
            name, module, requestId, correlationId, traceId, userId);

        await handler.HandleAsync(command, cancellationToken);
        
        logger.LogInformation("Handled a command: {Name} ({Module}) [Request ID: {RequestId}, Correlation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}']",
            name, module, requestId, correlationId, traceId, userId);
    }
}