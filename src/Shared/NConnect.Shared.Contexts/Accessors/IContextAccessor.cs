namespace NConnect.Shared.Contexts.Accessors;

public interface IContextAccessor
{
    IContext? Context { get; set; }
}