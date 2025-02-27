using NConnect.Shared.Common.Time;

namespace NConnect.Services.Chats.Tests.Unit;

internal sealed class TestTimeProvider : ITimeProvider
{
    public DateTime Now() => new(2025, 02, 27);
}