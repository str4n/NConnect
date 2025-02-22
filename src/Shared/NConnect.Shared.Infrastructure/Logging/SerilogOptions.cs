namespace NConnect.Shared.Infrastructure.Logging;

public sealed class SerilogOptions
{
    public string Level { get; set; } = "information";
    public ConsoleOptions Console { get; set; } = new();
    public SeqOptions Seq { get; set; } = new();
}

public sealed class ConsoleOptions
{
    public bool Enabled { get; set; }
    public string Template { get; set; } = string.Empty;
}

public sealed class SeqOptions
{
    public bool Enabled { get; set; }
    public string ConnectionString { get; set; } = string.Empty;
}