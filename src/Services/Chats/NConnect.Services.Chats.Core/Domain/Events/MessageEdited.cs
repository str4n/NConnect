namespace NConnect.Services.Chats.Core.Domain.Events;

public sealed record MessageEdited(Guid ChatId, Guid MessageId, Guid SenderId, string OldContent, string NewContent, DateTime Timestamp) 
    : Event(ChatId, Timestamp);