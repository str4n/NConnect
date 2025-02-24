using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NConnect.Shared.Common.Serialization;
using NConnect.Shared.Common.Time;

namespace NConnect.Shared.Common;

public static class Extensions
{
    private const string SectionName = "App";
    
    public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSingleton<ITimeProvider, UtcTimeProvider>()
            .AddSingleton<IJsonSerializer, DefaultJsonSerializer>()
            .Configure<AppOptions>(configuration.GetSection(SectionName))
            .AddSingleton(_ =>
            {
                var options = configuration.GetSection(SectionName).BindOptions<AppOptions>();
                return new AppInfo(options.Name, options.Version);
            });
    
    public static TOptions BindOptions<TOptions>(this IConfiguration configuration, string sectionName) where TOptions : class, new()
        => BindOptions<TOptions>(configuration.GetSection(sectionName));
    
    public static TOptions BindOptions<TOptions>(this IConfigurationSection section) where TOptions : class, new()
    {
        var options = new TOptions();
        section.Bind(options);

        return options;
    }
}