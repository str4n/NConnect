namespace NConnect.Shared.Contexts;

public interface IContextProvider
{
    IContext Current();
}