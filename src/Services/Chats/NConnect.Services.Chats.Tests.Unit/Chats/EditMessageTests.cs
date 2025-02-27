using NConnect.Services.Chats.Core.Domain;
using NConnect.Services.Chats.Core.Domain.Events;
using NConnect.Services.Chats.Core.Domain.Exceptions;
using NConnect.Shared.Common.Time;
using Shouldly;

namespace NConnect.Services.Chats.Tests.Unit.Chats;

public class EditMessageTests
{
    [Fact]
    public void Edit_Message_With_Valid_Args_Should_Be_Successful()
    {
        var chat = Chat.Create(new List<Guid>{SenderId, Guid.NewGuid()}, _timeProvider.Now());
        var messageId = Guid.NewGuid();
        var oldContent = "Test content";
        var newContent = "Edited content";
        var timestamp = _timeProvider.Now();
        chat.SendMessage(messageId, SenderId, oldContent, timestamp);
        
        chat.EditMessage(messageId, SenderId, oldContent, newContent, timestamp);
        
        var message = chat.Messages.Single();
        
        message.Content.ShouldBe(newContent);
        message.EditedAt.ShouldBe(timestamp);
        message.IsEdited.ShouldBeTrue();
        
        var @event = chat.Events.OfType<MessageEdited>().Single();
        @event.OldContent.ShouldBe(oldContent);
        @event.NewContent.ShouldBe(newContent);
    }

    [Fact]
    public void Edit_Message_With_Invalid_ParticipantId_Should_Throw_Exception()
    {
        var chat = Chat.Create(new List<Guid>{SenderId, Guid.NewGuid()}, _timeProvider.Now());
        var messageId = Guid.NewGuid();
        var oldContent = "Test content";
        var newContent = "Edited content";
        var timestamp = _timeProvider.Now();
        chat.SendMessage(messageId, SenderId, oldContent, timestamp);
        
        var action = () => chat.EditMessage(messageId, Guid.NewGuid(), oldContent, newContent, timestamp);
        
        action.ShouldThrow<InvalidParticipantException>();
    }
    
    [Fact]
    public void Edit_Message_With_Invalid_MessageId_Should_Throw_Exception()
    {
        var chat = Chat.Create(new List<Guid>{SenderId, Guid.NewGuid()}, _timeProvider.Now());
        var oldContent = "Test content";
        var newContent = "Edited content";
        var timestamp = _timeProvider.Now();
        
        var action = () => chat.EditMessage(Guid.NewGuid(), SenderId, oldContent, newContent, timestamp);
        
        action.ShouldThrow<MessageNotFoundException>();
    }
    
    [Fact]
    public void Edit_Message_With_Empty_New_Content_Should_Throw_Exception()
    {
        var chat = Chat.Create(new List<Guid>{SenderId, Guid.NewGuid()}, _timeProvider.Now());
        var messageId = Guid.NewGuid();
        var oldContent = "Test content";
        var newContent = string.Empty;
        var timestamp = _timeProvider.Now();
        chat.SendMessage(messageId, SenderId, oldContent, timestamp);
        
        var action = () => chat.EditMessage(messageId, SenderId, oldContent, newContent, timestamp);
        
        action.ShouldThrow<InvalidMessageContentException>();
    }

    [Fact]
    public void Edit_Message_With_Same_New_And_Old_Content_Should_Throw_Exception()
    {
        var chat = Chat.Create(new List<Guid>{SenderId, Guid.NewGuid()}, _timeProvider.Now());
        var messageId = Guid.NewGuid();
        var oldContent = "Test content";
        var newContent = oldContent;
        var timestamp = _timeProvider.Now();
        chat.SendMessage(messageId, SenderId, oldContent, timestamp);
        
        var action = () => chat.EditMessage(messageId, SenderId, oldContent, newContent, timestamp);
        
        action.ShouldThrow<CannotEditMessageException>();
    }
    
    private readonly ITimeProvider _timeProvider = new TestTimeProvider();
    private static readonly Guid SenderId = Guid.NewGuid();
}