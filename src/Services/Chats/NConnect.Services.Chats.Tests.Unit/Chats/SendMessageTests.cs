using NConnect.Services.Chats.Core.Domain;
using NConnect.Services.Chats.Core.Domain.Events;
using NConnect.Services.Chats.Core.Domain.Exceptions;
using NConnect.Shared.Common.Time;
using Shouldly;

namespace NConnect.Services.Chats.Tests.Unit.Chats;

public class SendMessageTests
{
    [Fact]
    public void Send_Message_With_Valid_Args_Should_Be_Successful()
    {
        var chat = Chat.Create(new List<Guid>{SenderId, Guid.NewGuid()}, _timeProvider.Now());
        var messageId = Guid.NewGuid();
        var content = "Test content";
        var timestamp = _timeProvider.Now();
        
        chat.SendMessage(messageId, SenderId, content, timestamp);

        chat.Messages.ShouldHaveSingleItem();
        var message = chat.Messages.Single();
        
        message.Id.ShouldBe(messageId);
        message.Content.ShouldBe(content);
        message.SentAt.ShouldBe(timestamp);
        
        var @event = chat.Events.OfType<MessageSent>().Single();
        @event.MessageId.ShouldBe(messageId);
    }

    [Fact]
    public void Send_Message_With_Invalid_SenderId_Should_Throw_Exception()
    {
        var chat = Chat.Create(new List<Guid>{SenderId, Guid.NewGuid()}, _timeProvider.Now());
        var messageId = Guid.NewGuid();
        var content = "Test content";
        var timestamp = _timeProvider.Now();
        
        var action = () => chat.SendMessage(messageId, Guid.NewGuid(), content, timestamp);

        action.ShouldThrow<InvalidParticipantException>();
    }
    
    [Fact]
    public void Send_Message_With_Empty_Content_Should_Throw_Exception()
    {
        var chat = Chat.Create(new List<Guid>{SenderId, Guid.NewGuid()}, _timeProvider.Now());
        var messageId = Guid.NewGuid();
        var content = string.Empty;
        var timestamp = _timeProvider.Now();
        
        var action = () => chat.SendMessage(messageId, SenderId, content, timestamp);

        action.ShouldThrow<InvalidMessageContentException>();
    }
        
    private readonly ITimeProvider _timeProvider = new TestTimeProvider();
    private static readonly Guid SenderId = Guid.NewGuid();
}