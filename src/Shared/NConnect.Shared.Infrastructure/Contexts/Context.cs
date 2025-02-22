using Microsoft.AspNetCore.Http;
using NConnect.Shared.Abstractions.Contexts;

namespace NConnect.Shared.Infrastructure.Contexts;

internal sealed class Context : IContext
{
    public Guid RequestId { get; } = Guid.NewGuid();
    public Guid CorrelationId { get; } = Guid.Empty;
    public string TraceId { get; } = string.Empty;
    public IIdentityContext Identity { get; } = IIdentityContext.Empty!;
    
    public Context(HttpContext context) : this(context.TryGetCorrelationId(), context.TraceIdentifier,
        new IdentityContext(context.User))
    {
    }

    private Context()
    {
        
    }

    private Context(Guid? correlationId, string traceId, IIdentityContext? identity = null)
    {
        CorrelationId = correlationId ?? Guid.NewGuid();
        TraceId = traceId;
        Identity = identity ?? IIdentityContext.Empty!;
    }

    public static IContext Empty => new Context();
}