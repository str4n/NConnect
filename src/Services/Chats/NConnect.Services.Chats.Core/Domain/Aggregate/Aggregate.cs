using NConnect.Services.Chats.Core.Domain.Events;

namespace NConnect.Services.Chats.Core.Domain.Aggregate;

public abstract class Aggregate<T>
{
    public T? Id { get; protected set; }
    protected readonly List<Event> _events = new();
    public IReadOnlyCollection<Event> Events => _events.AsReadOnly();
    public int Version { get; protected set; }
    protected abstract void ApplyEvent(Event @event);
}