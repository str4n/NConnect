using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace NConnect.Aspire.AppHost.Resources;

internal static class Extensions
{
    public static IResourceBuilder<T> WithScalarUi<T>(this IResourceBuilder<T> builder) where T : IResourceWithEndpoints
        => builder.WithOpenApiDocs("scalar-docs", "Scalar API Documentation", "scalar/v1");
    
    private static IResourceBuilder<T> WithOpenApiDocs<T>(this IResourceBuilder<T> builder,
        string name,
        string displayName,
        string openApiUiPath)
        where T : IResourceWithEndpoints
        => builder.WithCommand(name, displayName, executeCommand: _ =>
        {
            try
            {
                var endpoints = builder.Resource.GetEndpoints();
                var scheme = endpoints.Any(x => x.Scheme == "https") ? "https" : "http";
                
                var endpoint = builder.GetEndpoint(scheme);
                var url = $"{endpoint.Url}/{openApiUiPath}";

                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

                return Task.FromResult(new ExecuteCommandResult
                {
                    Success = true,
                });
            }
            catch (Exception e)
            {
                return Task.FromResult(new ExecuteCommandResult
                {
                    Success = false,
                    ErrorMessage = e.Message
                });
            }
        }, updateState: context 
            => context.ResourceSnapshot.HealthStatus == HealthStatus.Healthy 
            ? ResourceCommandState.Enabled 
            : ResourceCommandState.Disabled, 
            iconName: "Document", 
            iconVariant: IconVariant.Filled);
}