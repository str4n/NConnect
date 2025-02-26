using NConnect.Aspire.AppHost.Resources;
using Projects;

namespace NConnect.Aspire.AppHost;

internal static class Extensions
{
    public static IDistributedApplicationBuilder AddServices(this IDistributedApplicationBuilder builder)
    {
        var saga = builder.CreateProject<NConnect_Services_Saga>();

        var chats = builder.CreateProject<NConnect_Services_Chats_Api>();
        
        builder
            .CreateProject<NConnect_APIGateway>(1)
            .WithReference(saga)
            .WithReference(chats);
        
        return builder;
    }

    private static IResourceBuilder<ProjectResource> CreateProject<TProject>(
        this IDistributedApplicationBuilder builder, int index = 2)
        where TProject : IProjectMetadata, new()
        => builder
            .AddProject<TProject>(ExtractServiceName<TProject>(index))
            .WithScalarUi();

    private static string ExtractServiceName<T>(int index = 2)
        => typeof(T).Name.Split('_')[index];
}