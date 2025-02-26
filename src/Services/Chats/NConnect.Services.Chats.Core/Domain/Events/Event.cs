using NConnect.Shared.Common.Abstractions.DomainEvents;

namespace NConnect.Services.Chats.Core.Domain.Events;

public abstract record Event(Guid StreamId, DateTime Timestamp) : IDomainEvent;