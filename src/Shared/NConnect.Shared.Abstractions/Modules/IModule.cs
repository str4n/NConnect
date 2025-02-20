using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NConnect.Shared.Abstractions.Modules;

public interface IModule
{
    string Name { get; set; }
    void Add(IServiceCollection services, IConfiguration configuration);
    void Use(WebApplication app);
}