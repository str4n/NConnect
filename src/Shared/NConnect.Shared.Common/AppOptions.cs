namespace NConnect.Shared.Common;

public sealed class AppOptions
{
    public string Name { get; init; } = string.Empty;
    public int Version { get; init; }

    public AppOptions(string name, int version)
    {
        Name = name;
        Version = version;
    }

    public AppOptions()
    {
        
    }
};