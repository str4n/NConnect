namespace NConnect.Services.Chats.Core.Domain.Events;

public sealed record MessageSent(Guid ChatId, Guid MessageId, Guid SenderId, string Content, DateTime Timestamp) : Event(ChatId, Timestamp);