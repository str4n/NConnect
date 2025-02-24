using System.Text.Json;

namespace NConnect.Shared.Common.Serialization;

internal sealed class DefaultJsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };
    
    public string Serialize<T>(T value)
        => JsonSerializer.Serialize(value, _options);

    public T? Deserialize<T>(string value)
        => JsonSerializer.Deserialize<T>(value, _options);

    public object? Deserialize(string value, Type type)
        => JsonSerializer.Deserialize(value, type, _options);
}