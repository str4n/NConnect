using NConnect.Services.Chats.Core.Domain;
using NConnect.Services.Chats.Core.Domain.Events;
using NConnect.Services.Chats.Core.Domain.Exceptions;
using NConnect.Shared.Common.Time;
using Shouldly;

namespace NConnect.Services.Chats.Tests.Unit.Chats;

public class DeleteMessageTests
{
    [Fact]
    public void Delete_Message_With_Valid_Args_Should_Be_Successful()
    {
        var chat = Chat.Create(new List<Guid>{SenderId, Guid.NewGuid()}, _timeProvider.Now());
        var messageId = Guid.NewGuid();
        var content = "Test content";
        var timestamp = _timeProvider.Now();
        chat.SendMessage(messageId, SenderId, content, timestamp);
        
        chat.DeleteMessage(messageId, SenderId, timestamp);
        
        var message = chat.Messages.Single();
        message.DeletedAt.ShouldBe(timestamp);
        message.IsDeleted.ShouldBeTrue();
        
        var @event = chat.Events.OfType<MessageDeleted>().Single();
        @event.MessageId.ShouldBe(messageId);
    }

    [Fact]
    public void Delete_Message_With_Invalid_MessageId_Should_Throw_Exception()
    {
        var chat = Chat.Create(new List<Guid>{SenderId, Guid.NewGuid()}, _timeProvider.Now());
        var timestamp = _timeProvider.Now();
        
        var action = () => chat.DeleteMessage(Guid.NewGuid(), SenderId, timestamp);

        action.ShouldThrow<MessageNotFoundException>();
    }
    
    [Fact]
    public void Delete_Message_With_Invalid_ParticipantId_Should_Throw_Exception()
    {
        var chat = Chat.Create(new List<Guid>{SenderId, Guid.NewGuid()}, _timeProvider.Now());
        var messageId = Guid.NewGuid();
        var content = "Test content";
        var timestamp = _timeProvider.Now();
        chat.SendMessage(messageId, SenderId, content, timestamp);
        
        var action = () => chat.DeleteMessage(messageId, Guid.NewGuid(), timestamp);

        action.ShouldThrow<InvalidParticipantException>();
    }
    
    private readonly ITimeProvider _timeProvider = new TestTimeProvider();
    private static readonly Guid SenderId = Guid.NewGuid();
}