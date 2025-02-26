namespace NConnect.Services.Chats.Core.Domain;

public sealed record ChatId(Guid Value)
{
    public ChatId() : this(Guid.NewGuid())
    {
    }

    public static ChatId Create() => new(Guid.NewGuid());
    
    public static implicit operator Guid(ChatId id) => id.Value;
    public static implicit operator ChatId(Guid id) => new(id);
    
    public override string ToString() => Value.ToString();
}