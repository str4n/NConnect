namespace NConnect.Services.Chats.Core.Domain.Events;

public sealed record ChatCreated(Guid ChatId, IEnumerable<Guid> Participants, DateTime Timestamp) : Event(ChatId, Timestamp);