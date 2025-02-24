using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

namespace NConnect.Shared.Api.OpenApi;

public static class Extensions
{
    public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        => services.AddOpenApi();

    public static WebApplication MapApiDocumentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        return app;
    }
}