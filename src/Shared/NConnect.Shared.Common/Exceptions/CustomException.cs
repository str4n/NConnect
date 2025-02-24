namespace NConnect.Shared.Common.Exceptions;

public abstract class CustomException(string message, ExceptionCategory category) : Exception(message)
{
    public ExceptionCategory Category { get; } = category;
}
