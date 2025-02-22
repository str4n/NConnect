using System.Security.Claims;
using NConnect.Shared.Abstractions.Contexts;

namespace NConnect.Shared.Infrastructure.Contexts;

internal sealed class IdentityContext : IIdentityContext
{
    public bool IsAuthenticated { get; }
    public Guid Id { get; }
    public string Role { get; }
    public Dictionary<string, IEnumerable<string>> Claims { get; }

    private IdentityContext()
    {
    }

    public IdentityContext(Guid? id)
    {
        Id = id ?? Guid.Empty;
        IsAuthenticated = id.HasValue;
    }

    public IdentityContext(ClaimsPrincipal principal)
    {
        if (principal.Identity is null || string.IsNullOrWhiteSpace(principal.Identity.Name))
        {
            return;
        }
            
        IsAuthenticated = principal.Identity?.IsAuthenticated is true;
        Id = IsAuthenticated ? Guid.Parse(principal.Identity?.Name ?? string.Empty) : Guid.Empty;
        var role = principal.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        
        if (role != null)
        {
            Role = role;
            Claims = principal.Claims.GroupBy(x => x.Type)
                .ToDictionary(x => x.Key, x => x.Select(c => c.Value.ToString()));
        }
    }
        
    public static IIdentityContext Empty => new IdentityContext();
}