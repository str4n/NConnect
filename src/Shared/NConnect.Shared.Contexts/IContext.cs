namespace NConnect.Shared.Contexts;


public interface IContext
{
    string ActivityId { get; }
    string? UserId { get; }
    string? MessageId { get; }
}