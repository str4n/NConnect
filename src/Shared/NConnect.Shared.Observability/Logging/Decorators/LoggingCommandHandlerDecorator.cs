using Microsoft.Extensions.Logging;
using NConnect.Shared.Common;
using NConnect.Shared.Common.Abstractions.Commands;
using NConnect.Shared.Common.Attributes;
using NConnect.Shared.Contexts;

namespace NConnect.Shared.Observability.Logging.Decorators;

[Decorator]
internal sealed class LoggingCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> handler,
    ILogger<LoggingCommandHandlerDecorator<TCommand>> logger,
    IContextProvider contextProvider)
    : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        var context = contextProvider.Current();
        var commandName = typeof(TCommand).Name;
        
        logger.LogInformation("Handling a command: {CommandName} [Activity ID: {ActivityId}, Message ID: {MessageId}, User ID: {UserId}']...",
            commandName, context.ActivityId, context.MessageId, context.UserId);
        
        await handler.HandleAsync(command, cancellationToken);
        
        logger.LogInformation("Handled a command: {CommandName} [Activity ID: {ActivityId}, Message ID: {MessageId}, User ID: {UserId}]",
            commandName, context.ActivityId, context.MessageId, context.UserId);
    }
}