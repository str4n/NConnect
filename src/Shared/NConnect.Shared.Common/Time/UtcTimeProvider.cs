namespace NConnect.Shared.Common.Time;

internal sealed class UtcTimeProvider : ITimeProvider
{
    public DateTime Now() => DateTime.UtcNow;
}