using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NConnect.Shared.Api.Exceptions.Mappers;
using NConnect.Shared.Api.Exceptions.Middlewares;

namespace NConnect.Shared.Api.Exceptions;

public static class Extensions
{
    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        => services
            .AddSingleton<ExceptionHandlerMiddleware>()
            .AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
    
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlerMiddleware>();
}