namespace NConnect.Services.Chats.Core.Domain;

public sealed class Message
{
    public Guid Id { get; private set; }
    public Guid SenderId { get; private set; }
    public string Content { get; private set; }
    public DateTime SentAt { get; private set; }
    public bool IsEdited { get; private set; }
    public DateTime? EditedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public Message(Guid id, Guid senderId, string content, DateTime sentAt, bool isEdited = false, DateTime? editedAt = null, bool isDeleted = false, DateTime? deletedAt = null)
    {
        Id = id;
        SenderId = senderId;
        Content = content;
        SentAt = sentAt;
        IsEdited = isEdited;
        EditedAt = editedAt;
        IsDeleted = isDeleted;
        DeletedAt = deletedAt;
    }

    public void Edit(string newContent, DateTime timestamp)
    {
        Content = newContent;
        IsEdited = true;
        EditedAt = timestamp;
    }

    public void Delete(DateTime timestamp)
    {
        IsDeleted = true;
        DeletedAt = timestamp;
    }
}