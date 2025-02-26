using NConnect.Shared.Common.Exceptions;

namespace NConnect.Services.Chats.Core.Domain.Exceptions;

internal sealed class InvalidAggregateIdException(Guid value)
    : CustomException($"AggregateId: {value} is invalid.", ExceptionCategory.ValidationError);