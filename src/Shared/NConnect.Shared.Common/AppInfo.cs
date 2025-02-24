namespace NConnect.Shared.Common;

public sealed record AppInfo(string Name, int Version)
{
     public override string ToString() => $"{Name} {Version}";
}