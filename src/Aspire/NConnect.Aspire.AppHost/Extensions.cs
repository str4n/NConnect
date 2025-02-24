using NConnect.Aspire.AppHost.Resources;
using Projects;

namespace NConnect.Aspire.AppHost;

internal static class Extensions
{
    public static IDistributedApplicationBuilder AddServices(this IDistributedApplicationBuilder builder)
    {
        var saga = builder
            .AddProject<NConnect_Services_Saga>(GetServiceName<NConnect_Services_Saga>())
            .WithScalarUi();
        
        builder
            .AddProject<NConnect_APIGateway>(GetServiceName<NConnect_APIGateway>())
            .WithScalarUi()
            .WithReference(saga);
        
        return builder;
    }
    
    private static string GetServiceName<T>() => typeof(T).Name.Split('_').Last();
}