using NConnect.Shared.Common.Exceptions;

namespace NConnect.Services.Chats.Core.Domain.Exceptions;

internal sealed class InvalidParticipantException(string message) : CustomException(message, ExceptionCategory.BadRequest);