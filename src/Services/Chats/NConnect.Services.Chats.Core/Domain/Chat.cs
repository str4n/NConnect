using NConnect.Services.Chats.Core.Domain.Aggregate;
using NConnect.Services.Chats.Core.Domain.Events;
using NConnect.Services.Chats.Core.Domain.Exceptions;

namespace NConnect.Services.Chats.Core.Domain;

public sealed class Chat : Aggregate<ChatId>
{
    private readonly List<Message> _messages = new();
    public IReadOnlyCollection<Message> Messages => _messages;
    
    private HashSet<Guid> _participants = new();
    public IReadOnlyCollection<Guid> Participants => _participants;
    public DateTime CreatedAt { get; private set; }
    
    public static Chat Create(IEnumerable<Guid> participants, DateTime timestamp)
    {
        participants = participants.ToList();
        
        if (!participants.Any())
        {
            throw new InvalidChatParticipantsCountException("Chat must contain at least one participant.");
        }
        
        var id = ChatId.Create();

        var chat = new Chat();
        
        var @event = new ChatCreated(id, participants, timestamp);
        chat.ApplyEvent(@event);
        
        return chat;
    }

    public void SendMessage(Guid messageId, Guid senderId, string content, DateTime timestamp)
    {
        if (!_participants.Contains(senderId))
        {
            throw new InvalidParticipantException("The message sender is not part of this chat.");
        }
        
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new InvalidMessageContentException("Message cannot be empty.");
        }
        
        var @event = new MessageSent(Id!, messageId, senderId, content, timestamp);
        ApplyEvent(@event);
    }

    public void EditMessage(Guid messageId, Guid participantId, string oldContent, string newContent, DateTime timestamp)
    {
        if (!_participants.Contains(participantId))
        {
            throw new InvalidParticipantException("The message editor is not part of this chat.");
        }
        
        var message = _messages.FirstOrDefault(x => x.Id == messageId);
        
        if (message is null)
        {
            throw new MessageNotFoundException(messageId);
        }
        
        if (message.SenderId != participantId)
        {
            throw new InvalidParticipantException("Cannot delete the message.");
        }
        
        if (string.IsNullOrWhiteSpace(newContent))
        {
            throw new InvalidMessageContentException("Message cannot be empty.");
        }

        if (oldContent == newContent)
        {
            throw new CannotEditMessageException("New message content cannot be the same.");
        }
        
        var @event = new MessageEdited(Id!, messageId, participantId, oldContent, newContent, timestamp);
        ApplyEvent(@event);
    }

    public void DeleteMessage(Guid messageId, Guid participantId, DateTime timestamp)
    {
        if (!_participants.Contains(participantId))
        {
            throw new InvalidParticipantException("The message deleter is not part of this chat.");
        }
        
        var message = _messages.FirstOrDefault(x => x.Id == messageId);
        
        if (message is null)
        {
            throw new MessageNotFoundException(messageId);
        }

        if (message.SenderId != participantId)
        {
            throw new InvalidParticipantException("Cannot delete the message.");
        }
        
        var @event = new MessageDeleted(Id!, messageId, participantId, timestamp);
        ApplyEvent(@event);
    }

    public Chat ReplyEvents(IEnumerable<Event> events)
    {
        var chat = new Chat();
        
        foreach (var @event in events)
        {
            chat.ApplyEvent(@event);
        }
        
        return chat;
    }

    protected override void ApplyEvent(Event @event)
    {
        switch (@event)
        {
            case ChatCreated e:
                Apply(e);
                break;
            
            case MessageSent e:
                Apply(e);
                break;
            
            case MessageEdited e:
                Apply(e);
                break;
            
            case MessageDeleted e:
                Apply(e);
                break;
        }
        
        _events.Add(@event);
    }

    private void Apply(ChatCreated e)
    {
        Id = e.ChatId;
        _participants = e.Participants.ToHashSet();
        CreatedAt = e.Timestamp;
    }
    
    private void Apply(MessageSent e) 
        => _messages.Add(new Message(e.MessageId, e.SenderId, e.Content, e.Timestamp));
    
    private void Apply(MessageEdited e) 
        => _messages
            .Single(x => x.Id == e.MessageId)
            .Edit(e.NewContent, e.Timestamp);
    
    private void Apply(MessageDeleted e) 
        => _messages
            .Single(x => x.Id == e.MessageId)
            .Delete(e.Timestamp);
}