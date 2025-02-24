namespace NConnect.Shared.Common.Abstractions.DomainEvents;

public interface IDomainEventDispatcher
{
    Task DispatchAsync<TEvent>(TEvent @event) where TEvent : class, IDomainEvent;
}