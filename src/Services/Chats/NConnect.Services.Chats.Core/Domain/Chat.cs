using NConnect.Services.Chats.Core.Domain.Events;

namespace NConnect.Services.Chats.Core.Domain;

public sealed class Chat 
{
    public ChatId Id { get; private set; } = ChatId.Create();
    
    private readonly List<Message> _messages = new();
    public IEnumerable<Message> Messages => _messages;
    
    private HashSet<Guid> _participants = new();
    public IEnumerable<Guid> Participants => _participants;
    public readonly List<Event> Events = new();
    
    public static Chat Create(IEnumerable<Guid> participants, DateTime timestamp)
    {
        var listOfParticipants = participants.ToList();
        
        if (!listOfParticipants.Any())
        {
            //
        }
        
        var id = ChatId.Create();

        var chat = new Chat();
        
        var @event = new ChatCreated(id, listOfParticipants, timestamp);
        chat.Apply(@event);
        
        return chat;
    }

    private void Apply(Event @event)
    {
        switch (@event)
        {
            case ChatCreated e:
                Id = e.ChatId;
                _participants = e.Participants.ToHashSet();
                break;
            
            case MessageSent e:
                _messages.Add(new Message(e.ChatId, e.Content, e.Timestamp));
                break;
            
            case MessageEdited e:
                _messages
                    .Single(x => x.Id == e.MessageId)
                    .Edit(e.NewContent, e.Timestamp);
                break;
            
            case MessageDeleted e:
                _messages
                    .Single(x => x.Id == e.MessageId)
                    .Delete(e.Timestamp);
                break;
        }
        
        Events.Add(@event);
    }

    private static Chat ReplayEvents(IEnumerable<Event> events)
    {
        var chat = new Chat();
        
        foreach (var @event in events)
        {
            chat.Apply(@event);
        }
        
        return chat;
    }
}