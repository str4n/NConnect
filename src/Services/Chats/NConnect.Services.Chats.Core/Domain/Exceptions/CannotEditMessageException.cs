using NConnect.Shared.Common.Exceptions;

namespace NConnect.Services.Chats.Core.Domain.Exceptions;

internal sealed class CannotEditMessageException(string message) : CustomException(message, ExceptionCategory.ValidationError);