using Projects;

namespace NConnect.Aspire.AppHost;

internal static class Extensions
{
    public static IDistributedApplicationBuilder AddServices(this IDistributedApplicationBuilder builder)
    {
        var saga = builder.AddProject<NConnect_Services_Saga>(GetServiceName<NConnect_Services_Saga>());
        var apiGateway = builder.AddProject<NConnect_APIGateway>(GetServiceName<NConnect_APIGateway>())
            .WithReference(saga);
        
        return builder;
    }
    
    private static string GetServiceName<T>() => typeof(T).Name.Split('_').Last();
}