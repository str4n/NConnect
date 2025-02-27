using NConnect.Shared.Common.Exceptions;

namespace NConnect.Services.Chats.Core.Domain.Exceptions;

internal sealed class InvalidMessageContentException(string message) : CustomException(message, ExceptionCategory.ValidationError);