using NConnect.Shared.Common.Exceptions;

namespace NConnect.Services.Chats.Core.Domain.Exceptions;

internal sealed class MessageNotFoundException(Guid id) : CustomException($"Message with id: '{id}' was not found.", ExceptionCategory.NotFound);