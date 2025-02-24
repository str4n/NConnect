﻿namespace NConnect.Shared.Api.CORS;

internal sealed class CorsOptions
{
    public bool Enabled { get; set; }
    public bool AllowCredentials { get; set; }
    public IEnumerable<string> AllowedOrigins { get; set; } = new List<string>();
    public IEnumerable<string> AllowedMethods { get; set; } = new List<string>();
    public IEnumerable<string> AllowedHeaders { get; set; } = new List<string>();
    public IEnumerable<string> ExposedHeaders { get; set; } = new List<string>();
}