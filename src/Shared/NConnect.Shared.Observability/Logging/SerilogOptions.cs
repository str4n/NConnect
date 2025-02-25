namespace NConnect.Shared.Observability.Logging;

public sealed class SerilogOptions
{
    public string Level { get; init; } = "Information";
    public ConsoleOptions Console { get; init; } = new();
    public OpenTelemetryOptions OpenTelemetry { get; init; } = new();
}

public sealed class ConsoleOptions
{
    public bool Enabled { get; set; }
    public string Template { get; set; } = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
}


public sealed class OpenTelemetryOptions
{
    public bool Enabled { get; set; }
}