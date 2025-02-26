namespace NConnect.Services.Chats.Core.Domain.Events;

public sealed record MessageDeleted(Guid ChatId, Guid MessageId, Guid SenderId, DateTime Timestamp) : Event(ChatId, Timestamp);