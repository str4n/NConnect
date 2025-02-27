using NConnect.Services.Chats.Core.Domain;
using NConnect.Services.Chats.Core.Domain.Events;
using NConnect.Services.Chats.Core.Domain.Exceptions;
using NConnect.Shared.Common.Time;
using Shouldly;

namespace NConnect.Services.Chats.Tests.Unit.Chats;

public class CreateChatTests
{
    [Fact]
    public void Create_With_Valid_Args_Should_Return_Valid_Chat()
    {
        var firstParticipantId = Guid.NewGuid();
        var secondParticipantId = Guid.NewGuid();
        
        var participantIds = new List<Guid>
        {
            firstParticipantId, secondParticipantId
        };
        
        var timestamp = _timeProvider.Now();
        
        var chat = Chat.Create(participantIds, timestamp);
        
        chat.ShouldNotBeNull();
        chat.CreatedAt.ShouldBe(timestamp);
        chat.Participants.ToList().ShouldBeEquivalentTo(participantIds);
        
        chat.Events.ShouldHaveSingleItem();
        var @event = chat.Events.Single();
        @event.ShouldBeOfType<ChatCreated>();
        @event.ShouldBeEquivalentTo(new ChatCreated(chat.Id!, participantIds, timestamp));
    }

    [Fact]
    public void Create_With_Empty_ParticipantIds_Should_Throw_Exception()
    {
        var participantIds = Enumerable.Empty<Guid>();
        var timestamp = _timeProvider.Now();
        
        var action = () => Chat.Create(participantIds, timestamp);
        
        action.ShouldThrow<InvalidChatParticipantsCountException>();
    }
    
    private readonly ITimeProvider _timeProvider = new TestTimeProvider();
}